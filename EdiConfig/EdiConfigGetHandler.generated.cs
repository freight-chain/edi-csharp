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
    public partial class GetEdiConfigRequest : IRequest<EdiConfigServiceModel>
    {
        public string Id { get; set; }
    }
    public class EdiConfigGetHandlerBase : IRequestHandler<GetEdiConfigRequest,EdiConfigServiceModel>
    {
        public BaseRepository<EdiConfig> Repo { get; }
        public IMapperService Mapper { get; }

        public EdiConfigGetHandlerBase(
            BaseRepository<EdiConfig> repo,
            IMapperService mapper
            )
        {
            Repo = repo;
            Mapper = mapper;
        }

        public virtual async Task<EdiConfigServiceModel> Handle(GetEdiConfigRequest request, CancellationToken cancellationToken)
        {
            return Mapper.MapTo<EdiConfig,EdiConfigServiceModel>(await Repo.Find(request.Id),2);
        }

        protected virtual Expression<Func<EdiConfig, bool>> GetFilter()
        {
            return null;
        }
    }
}
