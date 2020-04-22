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
    public partial class AcceptRequest : IRequest<EdiTenderServiceModel>
    {
        public string Id { get; set; }

        
    }
     public partial class AcceptEvent : INotification
        {
            public string Id { get; set; }

            public EdiTenderServiceModel ServiceModel { get;set; }

            public EdiTender Model { get;set; }
        }
    public abstract class AcceptHandlerBase : IRequestHandler<AcceptRequest,EdiTenderServiceModel>
    {
        public BaseRepository<EdiTender> Repo { get; }
        public EdiTenderSearchEngine SearchEngine { get; }
        public IMapperService Mapper { get; }
        public IMediator Mediator { get; }

        public AcceptHandlerBase(
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

        public async Task<EdiTenderServiceModel> Handle(AcceptRequest request, CancellationToken cancellationToken)
        {
            var item = await Repo.Find(request.Id);
            await Handle(request, item);
            await Repo.Save(item);
            var serviceModel = Mapper.MapTo<EdiTender,EdiTenderServiceModel>(item,2);
            await Mediator.Publish(FillEvent(new AcceptEvent() { Id = request.Id, Model = item, ServiceModel = serviceModel }));
            return serviceModel;
        }

        protected abstract Task Handle(AcceptRequest request, EdiTender model);
        protected virtual AcceptEvent FillEvent(AcceptEvent evt) {
            return evt;
        }
    }
}
