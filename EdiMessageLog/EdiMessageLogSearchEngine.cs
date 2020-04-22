using System;
using System.Linq;
using System.Threading.Tasks;
using BlockArray.Core.Data;
using BlockArray.Core.Filtering;
using BlockArray.Core.Mapping;
using BlockArray.Core.Mongo;
using BlockArray.Core.Services;
using BlockArray.ServiceModel;

namespace FreightTrust.Modules.EdiMessageLog
{
    public partial class EdiMessageLogSearchEngine : BaseEdiMessageLogSearchEngine
    {
         
         [SearchFilter]
         public override IEntityFilter<EdiMessageLog> All()
         {
             return Where(p => p.Id != null);
         }
         
         [SearchFilter]
         public override IEntityFilter<EdiMessageLog> Incoming()
         {
             return Where(p => p.Type == EdiMessageType.Incoming);
         }
         
         [SearchFilter]
         public override IEntityFilter<EdiMessageLog> Outgoing()
         {
             return Where(p => p.Type == EdiMessageType.Outgoing);
         }
         
         [SearchFilter]
         public override IEntityFilter<EdiMessageLog> Processed()
         {
             return Where(p => p.Status == EdiMessageStatus.Processed);
         }
         
         [SearchFilter]
         public override IEntityFilter<EdiMessageLog> Failed()
         {
             return Where(p => p.Status == EdiMessageStatus.Failed);
         }
         
    }
}
