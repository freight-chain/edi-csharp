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
namespace FreightTrust.Modules.EdiConfig
{
    public static class EdiConfigExtensions {
        public static EdiConfigServiceModel MapEdiConfig(this IMapperService mapper, EdiConfig model, EdiConfigServiceModel serviceModel = null, int fidelity = 1) {
            serviceModel = serviceModel ?? new EdiConfigServiceModel();
            mapper.MapTo<EdiConfig,EdiConfigServiceModel>(model,serviceModel, fidelity);
            return serviceModel;
        }
    }
    public abstract class BaseEdiConfigMapper : IMapper<EdiConfig, EdiConfigServiceModel>, IFidelityMapper
    {
        public int Fidelity { get; set; }
        public IServiceProvider ServiceProvider { get;set; }

        public virtual EdiConfigServiceModel Map(EdiConfig source, EdiConfigServiceModel target)
        {
            if (source == null) return target;
            var mapper = ServiceProvider.GetService<IMapperService>();
            target.Id = source.Id;

            
            
            target.Enabled = source.Enabled;
            
            
            target.Username = source.Username;
            
            
            target.Password = source.Password;
            
            
            target.RsaKey = source.RsaKey;
            
            

            if (source.CompanyId != null) {
                    target.CompanyId = source.CompanyId;
                     if (Fidelity >= 1) {
                        target.Company = mapper.MapTo<FreightTrust.Modules.Company.Company, CompanyServiceModel>(
                                ServiceProvider.GetService<BaseRepository<FreightTrust.Modules.Company.Company>>().Get().FirstOrDefault( x=> x.Id == source.CompanyId), 0);
                    }
            }
            
            
            MapMore( source, target );
            return target;
        }
        public abstract void MapMore(EdiConfig source, EdiConfigServiceModel target);

        
        
        
        
        
        
        
        
        
        
        
    }

}
