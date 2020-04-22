using System.Threading.Tasks;
using BlockArray.Core.Services;
using BlockArray.ServiceModel;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;

namespace FreightTrust.Modules.EdiMessageLog
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    public class EdiMessageLogController : BaseEdiMessageLogController
    {
        public EdiMessageLogController(IMediator mediator) : base(mediator)
        {

        }
    }
}
