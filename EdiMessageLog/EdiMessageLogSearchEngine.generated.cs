using System;
using System.Linq;
using System.Threading.Tasks;
using BlockArray.Core.Data;
using BlockArray.Core.Mapping;
using BlockArray.Core.Mongo;
using BlockArray.Core.Services;
using BlockArray.ServiceModel;
using BlockArray.Core.Filtering;

namespace FreightTrust.Modules.EdiMessageLog
{
    public abstract class BaseEdiMessageLogSearchEngine : SearchEngine<EdiMessageLog>
    {
        
        
        [SearchFilter]
        public abstract IEntityFilter<EdiMessageLog> All();
        
        
        
        [SearchFilter]
        public abstract IEntityFilter<EdiMessageLog> Incoming();
        
        
        
        [SearchFilter]
        public abstract IEntityFilter<EdiMessageLog> Outgoing();
        
        
        
        [SearchFilter]
        public abstract IEntityFilter<EdiMessageLog> Processed();
        
        
        
        [SearchFilter]
        public abstract IEntityFilter<EdiMessageLog> Failed();
        
        
    }
}
