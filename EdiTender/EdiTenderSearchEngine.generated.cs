using System;
using System.Linq;
using System.Threading.Tasks;
using BlockArray.Core.Data;
using BlockArray.Core.Mapping;
using BlockArray.Core.Mongo;
using BlockArray.Core.Services;
using BlockArray.ServiceModel;
using BlockArray.Core.Filtering;

namespace FreightTrust.Modules.EdiTender
{
    public abstract class BaseEdiTenderSearchEngine : SearchEngine<EdiTender>
    {
        
        
        [SearchFilter]
        public abstract IEntityFilter<EdiTender> Incoming();
        
        
        
        [SearchFilter]
        public abstract IEntityFilter<EdiTender> Denied();
        
        
        
        [SearchFilter]
        public abstract IEntityFilter<EdiTender> Accepted();
        
        
    }
}
