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

namespace FreightTrust.Modules.EdiTender
{
    public partial class GetEdiTenderRequest : IRequest<EdiTenderServiceModel>
    {
        public string Id { get; set; }
    }
    public class EdiTenderGetHandlerBase : IRequestHandler<GetEdiTenderRequest,EdiTenderServiceModel>
    {
        public BaseRepository<EdiTender> Repo { get; }
        public IMapperService Mapper { get; }

        public EdiTenderGetHandlerBase(
            BaseRepository<EdiTender> repo,
            IMapperService mapper
            )
        {
            Repo = repo;
            Mapper = mapper;
        }

        public virtual async Task<EdiTenderServiceModel> Handle(GetEdiTenderRequest request, CancellationToken cancellationToken)
        {
            return Mapper.MapTo<EdiTender,EdiTenderServiceModel>(await Repo.Find(request.Id),2);
        }

        protected virtual Expression<Func<EdiTender, bool>> GetFilter()
        {
            return null;
        }
    }
}
