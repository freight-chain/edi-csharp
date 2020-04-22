using System;
using System.Threading.Tasks;
using EdiFabric.Core.Model.Edi;

namespace FreightTrust.Modules.EdiTender
{
    public interface IEdiHandler
    {
        Type ForType { get; }

        Task Handle(string companyId, TradingChannel.TradingChannel channel, TradingPartner.TradingPartner partner,
            object data, TradingPartnerMessage.TradingPartnerMessage message);
    }
}