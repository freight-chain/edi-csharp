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

namespace FreightTrust.Modules.EdiMessageLog
{
    public partial class DeleteEdiMessageLogRequest : IRequest
    {
        public string Id { get; set; }
    }
    public class EdiMessageLogDeleteHandlerBase : IRequestHandler<DeleteEdiMessageLogRequest>
    {
        public BaseRepository<EdiMessageLog> Repo { get; }
        public EdiMessageLogSearchEngine SearchEngine { get; }
        public IMapperService Mapper { get; }

        public EdiMessageLogDeleteHandlerBase(
            BaseRepository<EdiMessageLog> repo,
            EdiMessageLogSearchEngine searchEngine,
            IMapperService mapper
            )
        {
            Repo = repo;
            SearchEngine = searchEngine;
            Mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteEdiMessageLogRequest request, CancellationToken cancellationToken)
        {
            await Repo.Remove(await Repo.Find(request.Id));
            return Unit.Value;
        }

        protected virtual Expression<Func<EdiMessageLog, bool>> GetFilter()
        {
            return null;
        }
    }
}
