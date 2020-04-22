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
    public partial class SaveEdiConfigRequest : IRequest<EdiConfigServiceModel>
    {
        public EdiConfigServiceModel ServiceModel { get; set; }
    }
    public class EdiConfigSaveHandlerBase : IRequestHandler<SaveEdiConfigRequest,EdiConfigServiceModel>
    {
        public BaseRepository<EdiConfig> Repo { get; }
        public IMapperService Mapper { get; }

        public EdiConfigSaveHandlerBase(
            BaseRepository<EdiConfig> repo,
            IMapperService mapper
            )
        {
            Repo = repo;
            Mapper = mapper;
        }
        protected virtual async Task<EdiConfig> FindExisting(EdiConfigServiceModel requestServiceModel)
        {
            if (string.IsNullOrEmpty(requestServiceModel.Id))
                return null;

            return await Repo.Find(requestServiceModel.Id);
        }

        public virtual async Task<EdiConfigServiceModel> Handle(SaveEdiConfigRequest request, CancellationToken cancellationToken)
        {
            EdiConfig target = null;

            target = await FindExisting(request.ServiceModel) ?? new EdiConfig();


            if (target == null)
            {
                throw new Exception("Could not save the shipment with that id.  It was not found.");
            }
            var source = request.ServiceModel;
            target.Id = source.Id;
            
            
            target.Enabled = source.Enabled;
            
            
            
            target.Username = source.Username;
            
            
            
            target.Password = source.Password;
            
            
            
            target.RsaKey = source.RsaKey;
            
            
            
            target.CompanyId = source.CompanyId;
            
            
            await Apply(request.ServiceModel, target);
            // Save it
            await Repo.Save(target);

            return this.Mapper?.MapTo<EdiConfig, EdiConfigServiceModel>(target);
        }
        protected virtual async Task Apply(EdiConfigServiceModel source , EdiConfig target )
        {

        }
        protected virtual Expression<Func<EdiConfig, bool>> GetFilter()
        {
            return null;
        }
    }
}
