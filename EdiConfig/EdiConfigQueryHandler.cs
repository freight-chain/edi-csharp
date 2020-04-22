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

namespace FreightTrust.Modules.EdiConfig
{
    public class EdiConfigQueryHandler : EdiConfigQueryHandlerBase  {
        public EdiConfigQueryHandler(
                BaseRepository<EdiConfig> repo,
                EdiConfigSearchEngine searchEngine,
                IMapperService mapper
                ) : base(repo, searchEngine, mapper)
        {

        }
    }
}
