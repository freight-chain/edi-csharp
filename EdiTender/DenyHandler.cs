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
    public partial class DenyRequest : IRequest<EdiTenderServiceModel>
    {

    }

    public class DenyHandler : DenyHandlerBase  {
        public DenyHandler(
                BaseRepository<EdiTender> repo,
                EdiTenderSearchEngine searchEngine,
                IMapperService mapper,
                IMediator mediator
                ) : base(repo, searchEngine, mapper, mediator)
        {

        }

        protected override async Task Handle(DenyRequest request, EdiTender model)
        {
            model.Status = EdiStatus.Denied;
        }
    }
}
