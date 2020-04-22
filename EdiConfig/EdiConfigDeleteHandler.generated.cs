using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using BlockArray.Core.Data;
using BlockArray.Core.Mapping;
using BlockArray.Model.Mongo;
using BlockArray.ServiceModel;
using MediatR;

namespace FreightTrust.Modules.EdiConfig
{
    public partial class DeleteEdiConfigRequest : IRequest
    {
        public string Id { get; set; }
    }
    public class EdiConfigDeleteHandlerBase : IRequestHandler<DeleteEdiConfigRequest>
    {
        public BaseRepository<EdiConfig> Repo { get; }
        public EdiConfigSearchEngine SearchEngine { get; }
        public IMapperService Mapper { get; }

        public EdiConfigDeleteHandlerBase(
            BaseRepository<EdiConfig> repo,
            EdiConfigSearchEngine searchEngine,
            IMapperService mapper
            )
        {
            Repo = repo;
            SearchEngine = searchEngine;
            Mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteEdiConfigRequest request, CancellationToken cancellationToken)
        {
            await Repo.Remove(await Repo.Find(request.Id));
            return Unit.Value;
        }

        protected virtual Expression<Func<EdiConfig, bool>> GetFilter()
        {
            return null;
        }
    }
}
