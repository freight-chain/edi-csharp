using System.Linq;
using System.Threading.Tasks;
using BlockArray.ServiceModel;
using EdiFabric.Templates.X12004010;
using FreightTrust.EDI.Wrappers;
using FreightTrust.Handlers.Shipments;
using FreightTrust.Transactions;
using MediatR;

namespace FreightTrust.Modules.EdiTender
{
    public class EdiTenderResponseHandler : ShipmentEdiHandler<TS990>
    {
        public EdiTenderResponseHandler(ICompanyRepositoryProvider db, IMediator mediator) : base(db, mediator)
        {
        }

        protected override async Task HandleDocument(string companyId, TradingChannel.TradingChannel channel,
            TradingPartner.TradingPartner partner, TS990 ts990, TradingPartnerMessage.TradingPartnerMessage message)
        {
            var shipments = Db.Get<Shipment.Shipment>(companyId);
            var stp = Db.Get<ShipmentTradingPartner.ShipmentTradingPartner>(companyId);
            var transaction = AutoMapper.Mapper.Map<LoadTenderResponseTransaction>(ts990);

            var shipment = await shipments.FirstOrDefaultAsync(p => p.ShipmentIdentificationNumber == transaction.ShipmentIdentificationNumber);

            message.ReferenceId = shipment.ShipmentIdentificationNumber;

            var shipmentPartnerMessage = stp.Get()
                .FirstOrDefault(x => x.TradingPartnerId == partner.Id && x.ShipmentId == shipment.Id);

            if (transaction.Action == LoadTenderResponseAction.Accepted)
            {
                shipment.ToPartnerId = partner.Id;
                shipmentPartnerMessage.Status = ShipmentTradingPartnerStatus.Accepted;
                var evt = new ShipmentStatusChangedEvent()
                {
                    Previous = shipment.Status,
                    Shipment = shipment,
                    Sender = this
                };
                shipment.Status = evt.Status = ShipmentStatus.PendingDispatch;
                await Mediator.Publish(evt);
                
                // TODO Notify all other trading partners (if any) that the load is accepted by someone else
                
                
            } else if (transaction.Action == LoadTenderResponseAction.Declined)
            {
                shipmentPartnerMessage.Status = ShipmentTradingPartnerStatus.Declined;
//                var evt = new ShipmentStatusChangedEvent()
//                {
//                    Previous = shipment.Status,
//                    Shipment = shipment,
//                    Sender = this
//                };
//                shipment.Status = evt.Status = ShipmentStatus.Started;
//                await Mediator.Publish(evt);
            }
            else
            {
                shipment.ToPartnerId = null;
                shipmentPartnerMessage.Status = ShipmentTradingPartnerStatus.Cancelled;
                var evt = new ShipmentStatusChangedEvent()
                {
                    Previous = shipment.Status,
                    Shipment = shipment,
                    Sender = this
                };
                shipment.Status = evt.Status = ShipmentStatus.Started;
                await Mediator.Publish(evt);
                
            }

            await shipments.Save(shipment);
            await stp.Save(shipmentPartnerMessage);
        }
    }
}