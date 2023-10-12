using AuthorizationOperation.API.ViewModels;
using AuthorizationOperation.Application.UserCases.FindOne.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class AuthorizationsController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthorizationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Returns one Authorization according to ID
        /// </summary>
        /// <param name="id" example="1">Identifier from Authorization</param>
        /// <returns>Information result for one authorization</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/v1/authorizations/1
        ///         header Authorization: Bearer XXXXXXXXXXXXXXX
        /// </remarks>
        /// <response code="200">Request successful</response>
        /// <response code="401">The request is not validly authenticated</response>
        /// <response code="403">The client is not authorized for using this operation</response>
        /// <response code="404">The resource was not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorizationResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public async Task<IActionResult> Get(uint id)
        {
            var authorization = await this.mediator.Send(new AuthorizationGetQueryV1() { Id = id });
            if (authorization == null) return this.NotFound();

            return this.Ok(new AuthorizationResponse(authorization));
        }
    }
}
