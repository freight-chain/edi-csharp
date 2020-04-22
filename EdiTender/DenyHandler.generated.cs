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
    public partial class DenyRequest : IRequest<EdiTenderServiceModel>
    {
        public string Id { get; set; }

        
    }
     public partial class DenyEvent : INotification
        {
            public string Id { get; set; }

            public EdiTenderServiceModel ServiceModel { get;set; }

            public EdiTender Model { get;set; }
        }
    public abstract class DenyHandlerBase : IRequestHandler<DenyRequest,EdiTenderServiceModel>
    {
        public BaseRepository<EdiTender> Repo { get; }
        public EdiTenderSearchEngine SearchEngine { get; }
        public IMapperService Mapper { get; }
        public IMediator Mediator { get; }

        public DenyHandlerBase(
            BaseRepository<EdiTender> repo,
            EdiTenderSearchEngine searchEngine,
            IMapperService mapper,
            IMediator mediator
            )
        {
            Repo = repo;
            SearchEngine = searchEngine;
            Mapper = mapper;
            Mediator = mediator;
        }

        public async Task<EdiTenderServiceModel> Handle(DenyRequest request, CancellationToken cancellationToken)
        {
            var item = await Repo.Find(request.Id);
            await Handle(request, item);
            await Repo.Save(item);
            var serviceModel = Mapper.MapTo<EdiTender,EdiTenderServiceModel>(item,2);
            await Mediator.Publish(FillEvent(new DenyEvent() { Id = request.Id, Model = item, ServiceModel = serviceModel }));
            return serviceModel;
        }

        protected abstract Task Handle(DenyRequest request, EdiTender model);
        protected virtual DenyEvent FillEvent(DenyEvent evt) {
            return evt;
        }
    }
}
