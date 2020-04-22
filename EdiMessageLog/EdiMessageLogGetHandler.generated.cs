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

namespace FreightTrust.Modules.EdiMessageLog
{
    public partial class GetEdiMessageLogRequest : IRequest<EdiMessageLogServiceModel>
    {
        public string Id { get; set; }
    }
    public class EdiMessageLogGetHandlerBase : IRequestHandler<GetEdiMessageLogRequest,EdiMessageLogServiceModel>
    {
        public BaseRepository<EdiMessageLog> Repo { get; }
        public IMapperService Mapper { get; }

        public EdiMessageLogGetHandlerBase(
            BaseRepository<EdiMessageLog> repo,
            IMapperService mapper
            )
        {
            Repo = repo;
            Mapper = mapper;
        }

        public virtual async Task<EdiMessageLogServiceModel> Handle(GetEdiMessageLogRequest request, CancellationToken cancellationToken)
        {
            return Mapper.MapTo<EdiMessageLog,EdiMessageLogServiceModel>(await Repo.Find(request.Id),2);
        }

        protected virtual Expression<Func<EdiMessageLog, bool>> GetFilter()
        {
            return null;
        }
    }
}
