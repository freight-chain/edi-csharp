using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using BlockArray.Core.Data;
using BlockArray.Core.Mapping;
using BlockArray.Core.Services;
using BlockArray.Model.Mongo;
using BlockArray.ServiceModel;
using BlockArray.ServiceModel.PdfForms;
using BlockArray.ServiceModel.Shipments;
using FreightTrust.Interface.Shipments;
using MediatR;

namespace FreightTrust.Modules.EdiTender
{
    public partial class AcceptRequest : IRequest<EdiTenderServiceModel>
    {

    }
    public partial class AcceptEvent 
    {
        public ShipmentServiceModel Shipment { get; set; }
    }
    public class AcceptHandler : AcceptHandlerBase  {
        private readonly IMediator _mediator;
        private readonly BaseRepository<Shipper.Shipper> _shipperRepo;

        public AcceptHandler(
                BaseRepository<EdiTender> repo,
                EdiTenderSearchEngine searchEngine,
                IMapperService mapper,
                IMediator mediator,
                BaseRepository<Shipper.Shipper> shipperRepo
                ) : base(repo, searchEngine, mapper, mediator)
        {
            _mediator = mediator;
            _shipperRepo = shipperRepo;
        }

        protected override async Task Handle(AcceptRequest request, EdiTender model)
        {
            
        }
        public ShipmentServiceModel Shipment { get; set; }
        protected override AcceptEvent FillEvent(AcceptEvent evt)
        {
            evt.Shipment = Shipment;
            return base.FillEvent(evt);
        }

        private AddressServiceModel ConvertAddress(EdiTenderStopServiceModel f)
        {
            if (f == null) return new AddressServiceModel();
            return new AddressServiceModel()
            {
                City = f.City,
                State = f.State,
                Line1 = f.Address,
                Name = f.EntityName,
            };
        }
    }
}
