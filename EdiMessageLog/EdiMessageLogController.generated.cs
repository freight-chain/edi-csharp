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
    public class BaseEdiMessageLogController : Controller
    {
        public IMediator Mediator { get; }

        public BaseEdiMessageLogController( IMediator mediator )
        {
            Mediator = mediator;
        }
        

        

        
        [HttpPost("getEdiMessageLogs")]
        [Produces(typeof(QueryResult<EdiMessageLogServiceModel>))]
        [SwaggerOperation("getEdimessagelogs")]
        public async Task<IActionResult> GetEdiMessageLogs( [FromBody] EdiMessageLogQueryRequest request = null)
        {
            return Ok(await Mediator.Send(request));
        }
        

        
        [HttpGet("getEdiMessageLog")]
        [Produces(typeof(EdiMessageLogServiceModel))]
        [SwaggerOperation("getEdiMessageLog")]
        public async Task<IActionResult> GetEdiMessageLog(string id)
        {
            return Ok(await Mediator.Send(new GetEdiMessageLogRequest() { Id = id }));
        }
        

        
        [HttpPost("saveEdiMessageLog")]
        [Produces(typeof(EdiMessageLogServiceModel))]
        [SwaggerOperation("saveEdiMessageLog")]
        public async Task<IActionResult> SaveEdiMessageLog([FromBody] EdiMessageLogServiceModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await Mediator.Send(new SaveEdiMessageLogRequest() {
                ServiceModel = model
            }));
        }
        
        
        [HttpPost("deleteEdiMessageLog")]
        [SwaggerOperation("deleteEdiMessageLog")]
        public async Task<IActionResult> DeleteEdiMessageLog(string id)
        {
            await Mediator.Send(new DeleteEdiMessageLogRequest() { Id = id });
            return Ok();
        }
        
    }
}
