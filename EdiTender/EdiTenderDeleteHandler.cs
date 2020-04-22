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
    public class EdiTenderDeleteHandler : EdiTenderDeleteHandlerBase  {
        public EdiTenderDeleteHandler(
                BaseRepository<EdiTender> repo,
                EdiTenderSearchEngine searchEngine,
                IMapperService mapper
                ) : base(repo, searchEngine, mapper)
        {

        }
    }
}
