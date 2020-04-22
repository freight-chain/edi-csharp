using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BlockArray.Core;
using BlockArray.Core.Data;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Framework;
using EdiFabric.Framework.Readers;
using EdiFabric.Framework.Writers;
using EdiFabric.Sdk.Demo;
using EdiFabric.Templates.X12004010;
using FreightTrust.EDI.Wrappers;
using Newtonsoft.Json;
using TS997 = EdiFabric.Templates.X12004010.TS997;

namespace FreightTrust.Modules.EdiTender
{
    public class EdiProcessor
    {
        private readonly IEnumerable<IEdiHandler> _ediHandlers;
        private readonly IEnumerable<IEdiProvider> _ediProviders;
        private readonly ICompanyRepositoryProvider _db;

        public EdiProcessor(
            IEnumerable<IEdiHandler> ediHandlers,
            IEnumerable<IEdiProvider> ediProviders,
            ICompanyRepositoryProvider db
        )
        {
            _ediHandlers = ediHandlers;
            _ediProviders = ediProviders;
            _db = db;
        }

        public async Task<string> ProcessDocument(string companyId,
            TradingChannel.TradingChannel tradingChannel,
            TradingPartner.TradingPartner tradingPartner,
            string message, TradingPartnerMessage.TradingPartnerMessage messageData)
        {
            var ediStream = new MemoryStream(Encoding.Default.GetBytes(message));
            List<IEdiItem> ediItems;
            
            using (var reader = new X12Reader(ediStream, EdiHelper.TypeFactory))
            {
                var items = reader.ReadToEndAsync().Result;
                ediItems = items.ToList();
                string controlNumber = string.Empty;
                string errorMessage = null;
                foreach (var item in ediItems)
                {
                    if (item is ISA isa)
                    {
                        // find the company with the sender id
                        var senderId = isa.InterchangeSenderID_6.Trim();
                        var rId = isa.InterchangeReceiverID_8.Trim();
                        continue;
                        // VERIFY PARTNER INFORMATION IS CORRECT
                    }

                    if (item is GS gs)
                    {
                        controlNumber = gs.GroupControlNumber_6;
                        continue;
                    }

                    var handler = _ediHandlers.FirstOrDefault(p => p.ForType.IsAssignableFrom(item.GetType()));

                    if (handler != null)
                    {
                        try
                        {
                            if (item is EdiMessage edi)
                            {
                                if (edi.HasErrors)
                                {
                                    errorMessage = "EDI VALIDATION: " + string.Join(Environment.NewLine, edi.ErrorContext.Flatten());
                                    return errorMessage;
                                }
                            }
                            await handler.Handle(companyId, tradingChannel, tradingPartner, item, messageData);
                        }
                        catch (Exception ex)
                        {
                            errorMessage = ex.Message + Environment.NewLine + 
                                           ex.StackTrace + (ex.InnerException?.Message ?? string.Empty) + 
                                           (ex.InnerException?.StackTrace ?? string.Empty);
                            return errorMessage;
                        }
                    }
                }
            }

            return null;
        }

    }
}