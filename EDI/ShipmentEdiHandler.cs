using System.Threading.Tasks;
using MediatR;

namespace FreightTrust.Modules.EdiTender
{
    public abstract class ShipmentEdiHandler<TFor> : EdiHandler<TFor>
    {
        protected ICompanyRepositoryProvider Db { get; }
        protected IMediator Mediator { get; }

        public ShipmentEdiHandler(ICompanyRepositoryProvider db, IMediator mediator)
        {
            Db = db;
            Mediator = mediator;
        }

        protected abstract override Task HandleDocument(string companyId, TradingChannel.TradingChannel channel,
            TradingPartner.TradingPartner partner, TFor data, TradingPartnerMessage.TradingPartnerMessage message);
        
    }
}