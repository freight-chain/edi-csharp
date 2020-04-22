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
using IO.Swagger.Model;
using MediatR;

namespace FreightTrust.Modules.EdiMessageLog
{
    public partial class EdiMessageLogQueryRequest
    {
        public string ReferenceId { get; set; }
    }

    public class EdiMessageLogQueryHandler : EdiMessageLogQueryHandlerBase
    {
        public EdiMessageLogQueryHandler(
            BaseRepository<EdiMessageLog> repo,
            EdiMessageLogSearchEngine searchEngine,
            IMapperService mapper
        ) : base(repo, searchEngine, mapper)
        {
        }

        public override Task<QueryResult<EdiMessageLogServiceModel>> Handle(EdiMessageLogQueryRequest request, CancellationToken cancellationToken)
        {
            ReferenceId = request.ReferenceId;
            return base.Handle(request, cancellationToken);
        }

        public string ReferenceId { get; set; }
        protected override Expression<Func<EdiMessageLog, bool>> GetFilter()
        {
            if (!string.IsNullOrEmpty(ReferenceId))
            {
                return x => x.ReferenceId == ReferenceId;
            }
            return base.GetFilter();
        }
    }
}