using System.Threading.Tasks;
using BlockArray.Core.Services;
using BlockArray.ServiceModel;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
namespace FreightTrust.Modules.EdiTender
{
    public class BaseEdiTenderController : Controller
    {
        public IMediator Mediator { get; }

        public BaseEdiTenderController( IMediator mediator )
        {
            Mediator = mediator;
        }
        

        

        
        [HttpPost("getEdiTenders")]
        [Produces(typeof(QueryResult<EdiTenderServiceModel>))]
        [SwaggerOperation("getEditenders")]
        public async Task<IActionResult> GetEdiTenders( [FromBody] EdiTenderQueryRequest request = null)
        {
            return Ok(await Mediator.Send(request));
        }
        

        
        [HttpGet("getEdiTender")]
        [Produces(typeof(EdiTenderServiceModel))]
        [SwaggerOperation("getEdiTender")]
        public async Task<IActionResult> GetEdiTender(string id)
        {
            return Ok(await Mediator.Send(new GetEdiTenderRequest() { Id = id }));
        }
        

        
        [HttpPost("saveEdiTender")]
        [Produces(typeof(EdiTenderServiceModel))]
        [SwaggerOperation("saveEdiTender")]
        public async Task<IActionResult> SaveEdiTender([FromBody] EdiTenderServiceModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await Mediator.Send(new SaveEdiTenderRequest() {
                ServiceModel = model
            }));
        }
        
        
        [HttpPost("deleteEdiTender")]
        [SwaggerOperation("deleteEdiTender")]
        public async Task<IActionResult> DeleteEdiTender(string id)
        {
            await Mediator.Send(new DeleteEdiTenderRequest() { Id = id });
            return Ok();
        }
        
    }
}
