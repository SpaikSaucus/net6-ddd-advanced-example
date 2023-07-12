using AuthorizationOperation.API.ViewModels;
using AuthorizationOperation.Application.UserCases.Create.Command;
using AuthorizationOperation.Application.UserCases.FindAll.Queries;
using AuthorizationOperation.Application.UserCases.FindOne.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AuthorizationOperation.API.Controllers.V2.Customers
{
    /// <summary>
    /// This Endpoint use Explicit ApiVersion attribute so only will be served when the parameter api-version match.
    /// </summary>
    [ApiVersion("2.0")]
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
        /// Returns one Authorization according to UUID
        /// </summary>
        /// <param name="uuid" example="4bac8878-d319-4a8d-9648-87da3fbf2cc7">Identifier from Authorization</param>
        /// <returns>Information result for one authorization</returns>
        /// <remarks>
        /// This version 2.0 is the current one
        /// 
        /// Sample request:
        ///
        ///     GET /api/v1/Authorizations/4bac8878-d319-4a8d-9648-87da3fbf2cc7
        /// </remarks>
        /// <response code="200">Request successful</response>
        /// <response code="401">The request is not validly authenticated</response>
        /// <response code="403">The client is not authorized for using this operation</response>
        /// <response code="404">The resource was not found</response>
        [HttpGet("{uuid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorizationResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public async Task<IActionResult> GetOne(Guid uuid)
        {
            var authorization = await this.mediator.Send(new AuthorizationGetQuery() { UUID = uuid });
            if (authorization == null) return this.NotFound();

            return this.Ok(new AuthorizationResponse(authorization));
        }

        /// <summary>
        /// Returns paginated Authorization results
        /// </summary>
        /// <param name="criteria" example="{ &quot;statusIn&quot;: 4 }">Criteria filter</param>
        /// <param name="limit" example="200">Number of records per page.</param>
        /// <param name="offset" example="0">Results page you want to retrieve(0..N)</param>
        /// <param name="sort" example="customer,asc;id,desc">Sorting criteria in the format: property,(asc|desc). Default sort order is ascending. Multiple sort criteria are supported.</param>
        /// <returns>Information result for search authorizations</returns>
        /// <remarks>
        /// This method exists only in the current version, to show how the same "Controller" can evolve and maintain the versioning
        /// 
        /// Sample request:
        ///
        ///     GET /api/v2/Authorizations?sort=customer,asc;id,desc&amp;offset=0&amp;limit=200
        ///     {
        ///        "statusIn": 4
        ///     }
        /// </remarks>
        /// <response code="200">Request successful</response>
        /// <response code="401">The request is not validly authenticated</response>
        /// <response code="403">The client is not authorized for using this operation</response>
        /// <response code="404">The resource was not found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorizationPageResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public async Task<IActionResult> GetAll([FromBody] AuthorizationCriteriaRequest criteria, string sort, ushort offset, ushort limit)
        {
            var query = new AuthorizationGetAllQuery()
            {
                Limit = limit,
                Offset = offset,
                Sort = sort,
                StatusIn = criteria.ConvertToEnum(),
                StatusInDefaultSelected = criteria.StatusInDefaultSelected
            };

            var pageDto = await this.mediator.Send(query);

            return this.Ok(new AuthorizationPageResponse(pageDto));
        }

        /// <summary>
        /// Create a Authorization
        /// </summary>
        /// <returns>Information resulting from the creation authorization</returns>
        /// <remarks>
        /// This method exists only in the current version, to show how the same "Controller" can evolve and maintain the versioning
        /// 
        /// Sample request:
        ///
        ///     POST /api/v2/Authorizations
        ///     {
        ///         "UUID": "12345678-1234-1234-1234-123456789126",
        ///         "Customer": "Customer1"
        ///     }
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="401">The request is not validly authenticated</response>
        /// <response code="403">The client is not authorized for using this operation</response>
        /// <response code="404">The resource was not found</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateAuthorizationResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] CreateAuthorizationRequest req)
        {
            var response = await this.mediator.Send(new CreateAuthorizationCommand()
            {
                Customer = req.Customer,
                UUID = req.UUID
            });
            return this.Created($"/api/v2/Authorizations/{req.UUID}", new CreateAuthorizationResponse(response.Id));
        }
    }
}
