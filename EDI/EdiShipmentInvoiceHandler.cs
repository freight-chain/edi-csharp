using System.Threading.Tasks;
using EdiFabric.Templates.X12004010;
using FreightTrust.EDI.Wrappers;
using MediatR;

namespace FreightTrust.Modules.EdiTender
{
    public class EdiShipmentInvoiceHandler : ShipmentEdiHandler<TS210>
    {
        public EdiShipmentInvoiceHandler(ICompanyRepositoryProvider db, IMediator mediator) : base(db, mediator)
        {
        }

        protected override async Task HandleDocument(string companyId, TradingChannel.TradingChannel channel, TradingPartner.TradingPartner partner, TS210 ts210, TradingPartnerMessage.TradingPartnerMessage message)
        {
            var data = new LoadTenderInvoice(ts210);
            var shipments = Db.Get<Shipment.Shipment>(companyId);
            var shipment = await shipments.FirstOrDefaultAsync(p => p.ShipmentIdentificationNumber == data.ShipmentIdentificationNumber);
            message.ReferenceId = data.ShipmentIdentificationNumber;
            
            
            
        }
    }
}