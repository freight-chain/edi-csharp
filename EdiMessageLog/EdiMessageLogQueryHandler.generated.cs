using System;
using System.Collections.Generic;
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

namespace FreightTrust.Modules.EdiMessageLog
{
    public partial class EdiMessageLogQueryRequest : IRequest<QueryResult<EdiMessageLogServiceModel>>
    {
        public Query Query { get; set; }
    }
    public class EdiMessageLogQueryHandlerBase : IRequestHandler<EdiMessageLogQueryRequest,QueryResult<EdiMessageLogServiceModel>>
    {
        public BaseRepository<EdiMessageLog> Repo { get; }
        public EdiMessageLogSearchEngine SearchEngine { get; }
        public IMapperService Mapper { get; }

        public EdiMessageLogQueryHandlerBase(
            BaseRepository<EdiMessageLog> repo,
            EdiMessageLogSearchEngine searchEngine,
            IMapperService mapper
            )
        {
            Repo = repo;
            SearchEngine = searchEngine;
            Mapper = mapper;
        }

        protected async Task<QueryResult<EdiMessageLog>> QueryResult(Query query)
        {
            var filter = GetFilter();
            QueryResult<EdiMessageLog> results = null;
            if (filter != null)
            {
                results = await SearchEngine.Search(Repo.Get().Where(filter), query);
            }
            else
            {
                results = await SearchEngine.Search(Repo.Get(), query);
            }
            return results;
        }
        public virtual async Task<QueryResult<EdiMessageLogServiceModel>> Handle(EdiMessageLogQueryRequest request, CancellationToken cancellationToken)
        {
            return await Query(request.Query);
        }

        public virtual async Task<QueryResult<EdiMessageLogServiceModel>> Query(Query query = null)
        {
            var results = await QueryResult(query);
            var mapper = this.Mapper.Mapper<EdiMessageLog, EdiMessageLogServiceModel>(query.Fidelity);



            var result = new QueryResult<EdiMessageLogServiceModel>()
            {
                Result = new List<EdiMessageLogServiceModel>(),
                Total = results.Total
            };
            if (results.Result != null) {
                foreach (var item in results.Result) {
                    var mapped = mapper.Map(item, new EdiMessageLogServiceModel());
                    result.Result.Add(mapped);
                    await OnMap(item, mapped);
                }
            }
            return result;
        }
        public virtual async Task OnMap(EdiMessageLog model, EdiMessageLogServiceModel serviceModel) {

        }
        protected virtual Expression<Func<EdiMessageLog, bool>> GetFilter()
        {
            return null;
        }
    }
}
