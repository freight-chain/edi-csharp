using System;
using System.Linq;
using System.Threading.Tasks;
using BlockArray.Core.Data;
using BlockArray.Core.Filtering;
using BlockArray.Core.Mapping;
using BlockArray.Core.Mongo;
using BlockArray.Core.Services;
using BlockArray.ServiceModel;

namespace FreightTrust.Modules.EdiTender
{
    public partial class EdiTenderSearchEngine : BaseEdiTenderSearchEngine
    {
         
         [SearchFilter]
         public override IEntityFilter<EdiTender> Incoming()
         {
             return Where(x => x.Status == EdiStatus.Incoming);
         }
         
         [SearchFilter]
         public override IEntityFilter<EdiTender> Denied()
         {
             return Where(x => x.Status == EdiStatus.Denied);
         }
         
         [SearchFilter]
         public override IEntityFilter<EdiTender> Accepted()
         {
             return Where(x => x.Status == EdiStatus.Accepted);
         }
         
    }
}
