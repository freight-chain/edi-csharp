using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlockArray.Core.Data;
using BlockArray.Core.Mapping;
using BlockArray.Core.Mongo;
using BlockArray.Core.Services;
using BlockArray.ServiceModel;
using Microsoft.Extensions.DependencyInjection;
namespace FreightTrust.Modules.EdiTender
{
    public static class EdiTenderExtensions {
        public static EdiTenderServiceModel MapEdiTender(this IMapperService mapper, EdiTender model, EdiTenderServiceModel serviceModel = null, int fidelity = 1) {
            serviceModel = serviceModel ?? new EdiTenderServiceModel();
            mapper.MapTo<EdiTender,EdiTenderServiceModel>(model,serviceModel, fidelity);
            return serviceModel;
        }
    }
    public abstract class BaseEdiTenderMapper : IMapper<EdiTender, EdiTenderServiceModel>, IFidelityMapper
    {
        public int Fidelity { get; set; }
        public IServiceProvider ServiceProvider { get;set; }

        public virtual EdiTenderServiceModel Map(EdiTender source, EdiTenderServiceModel target)
        {
            if (source == null) return target;
            var mapper = ServiceProvider.GetService<IMapperService>();
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
            
            MapMore( source, target );
            return target;
        }
        public abstract void MapMore(EdiTender source, EdiTenderServiceModel target);

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
    }

}
