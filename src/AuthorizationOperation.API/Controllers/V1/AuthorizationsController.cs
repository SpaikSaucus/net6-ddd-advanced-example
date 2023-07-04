using AuthorizationOperation.Application.Shared.ViewModels;
using AuthorizationOperation.Application.UserCases.FindOne.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthorizationOperation.API.Controllers.V1.Customers
{
    /// <summary>
    /// This Endpoint use Explicit ApiVersion attribute so only will be served when the parameter api-version match.
    /// </summary>
    [ApiVersion("1.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthorizationsController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthorizationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks></remarks>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorizationResponse))]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public async Task<IActionResult> Get(uint id)
        {
            var response = await this.mediator.Send(new AuthorizationGetQueryV1() { Id = id });
            if (response == null) return this.NotFound();
            return this.Ok(response);
        }
    }
}
