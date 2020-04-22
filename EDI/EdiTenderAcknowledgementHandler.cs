using System.Threading.Tasks;
using EdiFabric.Templates.X12004010;
using FreightTrust.EDI.Wrappers;
using MediatR;

namespace FreightTrust.Modules.EdiTender
{
    public class EdiTenderAcknowledgementHandler : ShipmentEdiHandler<TS997>
    {
        public EdiTenderAcknowledgementHandler(ICompanyRepositoryProvider db, IMediator mediator) : base(db, mediator)
        {
        }

        protected override async Task HandleDocument(string companyId, TradingChannel.TradingChannel channel, TradingPartner.TradingPartner partner, TS997 ts997, TradingPartnerMessage.TradingPartnerMessage message)
        {
            var data = new Acknowledgment(ts997);
            //data
//            var shipments = Db.Get<Shipment.Shipment>(companyId);
//            var shipment = await shipments.FirstOrDefaultAsync(p =>
//                p.TradingPartnerId == partner.Id &&
//                p.ShipmentIdentificationNumber == data.);

            
            
        }
    }
}