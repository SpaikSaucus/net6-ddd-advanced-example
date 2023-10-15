using AuthorizationOperation.API.ViewModels;
using AuthorizationOperation.Application.UserCases.Create.Commands;
using AuthorizationOperation.Application.UserCases.FindAll.Queries;
using AuthorizationOperation.Application.UserCases.FindOne.Queries;
using AuthorizationOperation.Application.UserCases.Report.Commands;
using AuthorizationOperation.Infrastructure.Services.Accessor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AuthorizationOperation.API.Controllers.V2.Customers
{
    /// <summary>
    /// This Endpoint use Explicit ApiVersion attribute so only will be served when the parameter api-version match.
    /// </summary>
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorizationsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICurrentUserAccessor currentUser;

        public AuthorizationsController(IMediator mediator, ICurrentUserAccessor currentUser)
        {
            this.mediator = mediator;
            this.currentUser = currentUser;
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
        ///     GET /api/v1/authorizations/4bac8878-d319-4a8d-9648-87da3fbf2cc7
        ///         header Authorization: Bearer XXXXXXXXXXXXXXX
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
            // TODO: Can get GUID and validate or include business logical from Authorization Token
            // var guid = this.currentUser.User.Guid;

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
        ///     POST /api/v2/authorizations/findAllByCriteria?sort=customer,asc;id,desc&amp;offset=0&amp;limit=200
        ///     {
        ///        "statusIn": 4
        ///     }
        ///     header Authorization: Bearer XXXXXXXXXXXXXXX
        /// </remarks>
        /// <response code="200">Request successful</response>
        /// <response code="401">The request is not validly authenticated</response>
        /// <response code="403">The client is not authorized for using this operation</response>
        /// <response code="404">The resource was not found</response>
        [HttpPost("findAllByCriteria")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorizationPageResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public async Task<IActionResult> GetAll([FromBody] AuthorizationCriteriaRequest criteria, string sort, ushort offset, ushort limit)
        {
            // TODO: Can get GUID and validate or include business logical from Authorization Token
            // var guid = this.currentUser.User.Guid;

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
        ///     POST /api/v2/authorizations
        ///     {
        ///         "UUID": "12345678-1234-1234-1234-123456789126",
        ///         "Customer": "Customer1"
        ///     }
        ///     header Authorization: Bearer XXXXXXXXXXXXXXX
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
            // TODO: Can get GUID and validate or include business logical from Authorization Token
            // var guid = this.currentUser.User.Guid;

            var response = await this.mediator.Send(new CreateAuthorizationCommand()
            {
                Customer = req.Customer,
                UUID = req.UUID
            });
            return this.Created($"/api/v2/Authorizations/{req.UUID}", new CreateAuthorizationResponse(response.Id));
        }

        /// <summary>
        /// Download Report Excel
        /// </summary>
        /// <returns>Report Excel</returns>
        /// <remarks>
        /// This method exists only in the current version, to show how the same "Controller" can evolve and maintain the versioning
        /// 
        /// Sample request:
        ///
        ///     POST /api/v2/authorizations/report/excel
        ///     {
        ///        "statusIn": 4
        ///     }
        ///     header Authorization: Bearer XXXXXXXXXXXXXXX
        /// </remarks>
        /// <response code="200">Request successful</response>
        /// <response code="401">The request is not validly authenticated</response>
        /// <response code="403">The client is not authorized for using this operation</response>
        /// <response code="404">The resource was not found</response>
        [HttpPost("report/excel")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(File))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReportExcel([FromBody] AuthorizationCriteriaRequest criteria)
        {
            var cmd = new ReportFileExcelCommand()
            {
                StatusIn = criteria.ConvertToEnum()
            };

            var stream = await this.mediator.Send(cmd);
            return this.File(stream.ToArray(), "application/octet-stream", "reportAuthOp.xlsx");
        }
    }
}
