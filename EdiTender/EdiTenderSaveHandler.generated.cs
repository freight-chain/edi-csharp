using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using BlockArray.Core.Data;
using BlockArray.Core.Mapping;
using BlockArray.Core.Services;
using BlockArray.Model.Mongo;
using BlockArray.ServiceModel;
using MediatR;

namespace FreightTrust.Modules.EdiTender
{
    public partial class SaveEdiTenderRequest : IRequest<EdiTenderServiceModel>
    {
        public EdiTenderServiceModel ServiceModel { get; set; }
    }
    public class EdiTenderSaveHandlerBase : IRequestHandler<SaveEdiTenderRequest,EdiTenderServiceModel>
    {
        public BaseRepository<EdiTender> Repo { get; }
        public IMapperService Mapper { get; }

        public EdiTenderSaveHandlerBase(
            BaseRepository<EdiTender> repo,
            IMapperService mapper
            )
        {
            Repo = repo;
            Mapper = mapper;
        }
        protected virtual async Task<EdiTender> FindExisting(EdiTenderServiceModel requestServiceModel)
        {
            if (string.IsNullOrEmpty(requestServiceModel.Id))
                return null;

            return await Repo.Find(requestServiceModel.Id);
        }

        public virtual async Task<EdiTenderServiceModel> Handle(SaveEdiTenderRequest request, CancellationToken cancellationToken)
        {
            EdiTender target = null;

            target = await FindExisting(request.ServiceModel) ?? new EdiTender();


            if (target == null)
            {
                throw new Exception("Could not save the shipment with that id.  It was not found.");
            }
            var source = request.ServiceModel;
            target.Id = source.Id;
            
            
            target.ControlNumber = source.ControlNumber;
            
            
            
            target.Scac = source.Scac;
            
            
            
            target.BillOfLading = source.BillOfLading;
            
            
            
            target.ShipmentOwnerId = source.ShipmentOwnerId;
            
            
            
            target.LoadTenderType = source.LoadTenderType;
            
            
            
            target.MethodOfPayment = source.MethodOfPayment;
            
            
            
            target.TenderDateTime = source.TenderDateTime;
            
            
            
            target.TotalWeight = source.TotalWeight;
            
            
            
            target.TotalQuantity = source.TotalQuantity;
            
            
            
            target.TotalCharges = source.TotalCharges;
            
            
            
            target.Notes = source.Notes;
            
            
            
            target.EquipmentNumber = source.EquipmentNumber;
            
            
            
            target.ReferenceInfo = source.ReferenceInfo;
            
            
            
            target.EntityCode = source.EntityCode;
            
            
            
            target.EntityName = source.EntityName;
            
            
            
            target.LocationCode = source.LocationCode;
            
            
            
            target.EdiDocument = source.EdiDocument;
            
            
            
            target.Status = source.Status;
            
            
            
            target.ShipmentId = source.ShipmentId;
            
            
            await Apply(request.ServiceModel, target);
            // Save it
            await Repo.Save(target);

            return this.Mapper?.MapTo<EdiTender, EdiTenderServiceModel>(target);
        }
        protected virtual async Task Apply(EdiTenderServiceModel source , EdiTender target )
        {

        }
        protected virtual Expression<Func<EdiTender, bool>> GetFilter()
        {
            return null;
        }
    }
}
