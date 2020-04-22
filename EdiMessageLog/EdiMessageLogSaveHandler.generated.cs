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
    public partial class SaveEdiMessageLogRequest : IRequest<EdiMessageLogServiceModel>
    {
        public EdiMessageLogServiceModel ServiceModel { get; set; }
    }
    public class EdiMessageLogSaveHandlerBase : IRequestHandler<SaveEdiMessageLogRequest,EdiMessageLogServiceModel>
    {
        public BaseRepository<EdiMessageLog> Repo { get; }
        public IMapperService Mapper { get; }

        public EdiMessageLogSaveHandlerBase(
            BaseRepository<EdiMessageLog> repo,
            IMapperService mapper
            )
        {
            Repo = repo;
            Mapper = mapper;
        }
        protected virtual async Task<EdiMessageLog> FindExisting(EdiMessageLogServiceModel requestServiceModel)
        {
            if (string.IsNullOrEmpty(requestServiceModel.Id))
                return null;

            return await Repo.Find(requestServiceModel.Id);
        }

        public virtual async Task<EdiMessageLogServiceModel> Handle(SaveEdiMessageLogRequest request, CancellationToken cancellationToken)
        {
            EdiMessageLog target = null;

            target = await FindExisting(request.ServiceModel) ?? new EdiMessageLog();


            if (target == null)
            {
                throw new Exception("Could not save the shipment with that id.  It was not found.");
            }
            var source = request.ServiceModel;
            target.Id = source.Id;
            
            
            target.ReferenceId = source.ReferenceId;
            
            
            
            target.Type = source.Type;
            
            
            
            target.Status = source.Status;
            
            
            
            target.EdiTypeNumber = source.EdiTypeNumber;
            
            
            
            target.MessageTypeName = source.MessageTypeName;
            
            
            
            target.TradingChannelId = source.TradingChannelId;
            
            
            
            target.TradingPartnerId = source.TradingPartnerId;
            
            
            
            target.Content = source.Content;
            
            
            
            target.ErrorMessage = source.ErrorMessage;
            
            
            await Apply(request.ServiceModel, target);
            // Save it
            await Repo.Save(target);

            return this.Mapper?.MapTo<EdiMessageLog, EdiMessageLogServiceModel>(target);
        }
        protected virtual async Task Apply(EdiMessageLogServiceModel source , EdiMessageLog target )
        {

        }
        protected virtual Expression<Func<EdiMessageLog, bool>> GetFilter()
        {
            return null;
        }
    }
}
