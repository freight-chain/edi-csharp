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

namespace FreightTrust.Modules.EdiTender
{
    public partial class EdiTenderQueryRequest : IRequest<QueryResult<EdiTenderServiceModel>>
    {
        public Query Query { get; set; }
    }
    public class EdiTenderQueryHandlerBase : IRequestHandler<EdiTenderQueryRequest,QueryResult<EdiTenderServiceModel>>
    {
        public BaseRepository<EdiTender> Repo { get; }
        public EdiTenderSearchEngine SearchEngine { get; }
        public IMapperService Mapper { get; }

        public EdiTenderQueryHandlerBase(
            BaseRepository<EdiTender> repo,
            EdiTenderSearchEngine searchEngine,
            IMapperService mapper
            )
        {
            Repo = repo;
            SearchEngine = searchEngine;
            Mapper = mapper;
        }

        protected async Task<QueryResult<EdiTender>> QueryResult(Query query)
        {
            var filter = GetFilter();
            QueryResult<EdiTender> results = null;
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
        public virtual async Task<QueryResult<EdiTenderServiceModel>> Handle(EdiTenderQueryRequest request, CancellationToken cancellationToken)
        {
            return await Query(request.Query);
        }

        public virtual async Task<QueryResult<EdiTenderServiceModel>> Query(Query query = null)
        {
            var results = await QueryResult(query);
            var mapper = this.Mapper.Mapper<EdiTender, EdiTenderServiceModel>(query.Fidelity);



            var result = new QueryResult<EdiTenderServiceModel>()
            {
                Result = new List<EdiTenderServiceModel>(),
                Total = results.Total
            };
            if (results.Result != null) {
                foreach (var item in results.Result) {
                    var mapped = mapper.Map(item, new EdiTenderServiceModel());
                    result.Result.Add(mapped);
                    await OnMap(item, mapped);
                }
            }
            return result;
        }
        public virtual async Task OnMap(EdiTender model, EdiTenderServiceModel serviceModel) {

        }
        protected virtual Expression<Func<EdiTender, bool>> GetFilter()
        {
            return null;
        }
    }
}
