using System;
using System.Threading.Tasks;
using EdiFabric.Core.Model.Edi;

namespace FreightTrust.Modules.EdiTender
{
    public abstract class EdiHandler<TFor> : IEdiHandler
    {
        public Type ForType => typeof(TFor);

        async Task IEdiHandler.Handle(string companyId, TradingChannel.TradingChannel channel,
            TradingPartner.TradingPartner partner, object data, TradingPartnerMessage.TradingPartnerMessage message)
        {
            await HandleDocument(companyId, channel, partner, (TFor) data, message);
        }

        protected abstract Task HandleDocument(string companyId, TradingChannel.TradingChannel channel,
            TradingPartner.TradingPartner partner, TFor data, TradingPartnerMessage.TradingPartnerMessage message);
    }
}