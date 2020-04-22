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

namespace FreightTrust.Modules.EdiConfig
{
    public partial class EdiConfigQueryRequest : IRequest<QueryResult<EdiConfigServiceModel>>
    {
        public Query Query { get; set; }
    }
    public class EdiConfigQueryHandlerBase : IRequestHandler<EdiConfigQueryRequest,QueryResult<EdiConfigServiceModel>>
    {
        public BaseRepository<EdiConfig> Repo { get; }
        public EdiConfigSearchEngine SearchEngine { get; }
        public IMapperService Mapper { get; }

        public EdiConfigQueryHandlerBase(
            BaseRepository<EdiConfig> repo,
            EdiConfigSearchEngine searchEngine,
            IMapperService mapper
            )
        {
            Repo = repo;
            SearchEngine = searchEngine;
            Mapper = mapper;
        }

        protected async Task<QueryResult<EdiConfig>> QueryResult(Query query)
        {
            var filter = GetFilter();
            QueryResult<EdiConfig> results = null;
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
        public virtual async Task<QueryResult<EdiConfigServiceModel>> Handle(EdiConfigQueryRequest request, CancellationToken cancellationToken)
        {
            return await Query(request.Query);
        }

        public virtual async Task<QueryResult<EdiConfigServiceModel>> Query(Query query = null)
        {
            var results = await QueryResult(query);
            var mapper = this.Mapper.Mapper<EdiConfig, EdiConfigServiceModel>(query.Fidelity);



            var result = new QueryResult<EdiConfigServiceModel>()
            {
                Result = new List<EdiConfigServiceModel>(),
                Total = results.Total
            };
            if (results.Result != null) {
                foreach (var item in results.Result) {
                    var mapped = mapper.Map(item, new EdiConfigServiceModel());
                    result.Result.Add(mapped);
                    await OnMap(item, mapped);
                }
            }
            return result;
        }
        public virtual async Task OnMap(EdiConfig model, EdiConfigServiceModel serviceModel) {

        }
        protected virtual Expression<Func<EdiConfig, bool>> GetFilter()
        {
            return null;
        }
    }
}
