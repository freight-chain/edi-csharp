using System.Threading.Tasks;
using BlockArray.Core.Services;
using BlockArray.ServiceModel;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
namespace FreightTrust.Modules.EdiConfig
{
    public class BaseEdiConfigController : Controller
    {
        public IMediator Mediator { get; }

        public BaseEdiConfigController( IMediator mediator )
        {
            Mediator = mediator;
        }
        

        

        
        [HttpPost("getEdiConfigs")]
        [Produces(typeof(QueryResult<EdiConfigServiceModel>))]
        [SwaggerOperation("getEdiconfigs")]
        public async Task<IActionResult> GetEdiConfigs( [FromBody] EdiConfigQueryRequest request = null)
        {
            return Ok(await Mediator.Send(request));
        }
        

        
        [HttpGet("getEdiConfig")]
        [Produces(typeof(EdiConfigServiceModel))]
        [SwaggerOperation("getEdiConfig")]
        public async Task<IActionResult> GetEdiConfig(string id)
        {
            return Ok(await Mediator.Send(new GetEdiConfigRequest() { Id = id }));
        }
        

        
        [HttpPost("saveEdiConfig")]
        [Produces(typeof(EdiConfigServiceModel))]
        [SwaggerOperation("saveEdiConfig")]
        public async Task<IActionResult> SaveEdiConfig([FromBody] EdiConfigServiceModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await Mediator.Send(new SaveEdiConfigRequest() {
                ServiceModel = model
            }));
        }
        
        
        [HttpPost("deleteEdiConfig")]
        [SwaggerOperation("deleteEdiConfig")]
        public async Task<IActionResult> DeleteEdiConfig(string id)
        {
            await Mediator.Send(new DeleteEdiConfigRequest() { Id = id });
            return Ok();
        }
        
    }
}
