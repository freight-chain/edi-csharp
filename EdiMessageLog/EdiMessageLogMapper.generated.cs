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
namespace FreightTrust.Modules.EdiMessageLog
{
    public static class EdiMessageLogExtensions {
        public static EdiMessageLogServiceModel MapEdiMessageLog(this IMapperService mapper, EdiMessageLog model, EdiMessageLogServiceModel serviceModel = null, int fidelity = 1) {
            serviceModel = serviceModel ?? new EdiMessageLogServiceModel();
            mapper.MapTo<EdiMessageLog,EdiMessageLogServiceModel>(model,serviceModel, fidelity);
            return serviceModel;
        }
    }
    public abstract class BaseEdiMessageLogMapper : IMapper<EdiMessageLog, EdiMessageLogServiceModel>, IFidelityMapper
    {
        public int Fidelity { get; set; }
        public IServiceProvider ServiceProvider { get;set; }

        public virtual EdiMessageLogServiceModel Map(EdiMessageLog source, EdiMessageLogServiceModel target)
        {
            if (source == null) return target;
            var mapper = ServiceProvider.GetService<IMapperService>();
            target.Id = source.Id;

            
            
            target.ReferenceId = source.ReferenceId;
            
            
            target.Type = source.Type;
            
            
            target.Status = source.Status;
            
            
            target.EdiTypeNumber = source.EdiTypeNumber;
            
            
            target.MessageTypeName = source.MessageTypeName;
            
            

            if (source.TradingChannelId != null) {
                    target.TradingChannelId = source.TradingChannelId;
                     if (Fidelity >= 1) {
                        target.TradingChannel = mapper.MapTo<FreightTrust.Modules.TradingChannel.TradingChannel, TradingChannelServiceModel>(
                                ServiceProvider.GetService<BaseRepository<FreightTrust.Modules.TradingChannel.TradingChannel>>().Get().FirstOrDefault( x=> x.Id == source.TradingChannelId), 0);
                    }
            }
            
            
            

            if (source.TradingPartnerId != null) {
                    target.TradingPartnerId = source.TradingPartnerId;
                     if (Fidelity >= 1) {
                        target.TradingPartner = mapper.MapTo<FreightTrust.Modules.TradingPartner.TradingPartner, TradingPartnerServiceModel>(
                                ServiceProvider.GetService<BaseRepository<FreightTrust.Modules.TradingPartner.TradingPartner>>().Get().FirstOrDefault( x=> x.Id == source.TradingPartnerId), 0);
                    }
            }
            
            
            
            target.Content = source.Content;
            
            
            target.ErrorMessage = source.ErrorMessage;
            
            MapMore( source, target );
            return target;
        }
        public abstract void MapMore(EdiMessageLog source, EdiMessageLogServiceModel target);

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
    }

}
