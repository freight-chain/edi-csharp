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

namespace FreightTrust.Modules.EdiTender
{
    public partial class DeleteEdiTenderRequest : IRequest
    {
        public string Id { get; set; }
    }
    public class EdiTenderDeleteHandlerBase : IRequestHandler<DeleteEdiTenderRequest>
    {
        public BaseRepository<EdiTender> Repo { get; }
        public EdiTenderSearchEngine SearchEngine { get; }
        public IMapperService Mapper { get; }

        public EdiTenderDeleteHandlerBase(
            BaseRepository<EdiTender> repo,
            EdiTenderSearchEngine searchEngine,
            IMapperService mapper
            )
        {
            Repo = repo;
            SearchEngine = searchEngine;
            Mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteEdiTenderRequest request, CancellationToken cancellationToken)
        {
            await Repo.Remove(await Repo.Find(request.Id));
            return Unit.Value;
        }

        protected virtual Expression<Func<EdiTender, bool>> GetFilter()
        {
            return null;
        }
    }
}
