using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlockArray.Core;
using BlockArray.Core.Data;
using BlockArray.ServiceModel;
using EdiFabric.Framework;
using EdiFabric.Framework.Writers;
using FreightTrust.EDI;
using FreightTrust.EDI.Wrappers;
using FreightTrust.Modules.EdiTender;
using FreightTrust.Modules.Shipment;
using FreightTrust.Modules.Tender;
using MediatR;

namespace FreightTrust.Modules.EdiMessageLog
{
    public class HandleEdiMessages : INotificationHandler<AcceptTenderEvent>, 
        INotificationHandler<BeginEvent>, INotificationHandler<CompleteEvent>
    {
        private readonly BaseRepository<Shipper.Shipper> _shipperRepo;
        private readonly BaseRepository<EdiMessageLog> _ediMessages;
        private readonly IUserContextProvider _userContext;
        private readonly ITradingPartnerSender _sender;
        private readonly ICompanyRepositoryProvider _db;
        private readonly IMediator _mediator;

        public HandleEdiMessages(
            BaseRepository<Shipper.Shipper> shipperRepo,
            BaseRepository<EdiMessageLog> ediMessages,
            IUserContextProvider userContext,
            ITradingPartnerSender sender,
            ICompanyRepositoryProvider db,
            IMediator mediator
            )
        {
            _shipperRepo = shipperRepo;
            _ediMessages = ediMessages;
            _userContext = userContext;
            _sender = sender;
            _db = db;
            _mediator = mediator;
        }

        public async Task Handle(AcceptTenderEvent notification, CancellationToken cancellationToken)
        {
    
        }

        public async Task Handle(BeginEvent notification, CancellationToken cancellationToken)
        {
//            await _mediator.Send(new ShipmentStatusUpdateRequest()
//            {
//                Id = notification.ServiceModel.Id,
//                ServiceModel = new ShipmentStatusUpdateServiceModel()
//                {
//                    StatusDate = DateTime.UtcNow,
//                    StatusCodeId = "P1",
//                }
//            }, cancellationToken);
        }

        public async Task Handle(CompleteEvent notification, CancellationToken cancellationToken)
        {
            //var partnerId = notification.Model.TradingPartnerId;
            //var participant = _db.Get<InvoiceParticipant.InvoiceParticipant>().FirstOrDefault(x => x.ReferenceId == partnerId);

            //var _db.Get<AccountingInvoice.AccountingInvoice>().FirstOrDefault(x => x.ToId == participant);
//            await _mediator.Send(new ShipmentStatusUpdateRequest()
//            {
//                Id = notification.ServiceModel.Id,
//                ServiceModel = new ShipmentStatusUpdateServiceModel()
//                {
//                    StatusDate = DateTime.UtcNow,
//                    StatusCodeId = "CD",
//                }
//            }, cancellationToken);

            //var invoice = new LoadTenderInvoice();
            
            

        }
    }
}