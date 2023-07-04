using AuthorizationOperation.Application.Shared.ViewModels;
using AuthorizationOperation.Application.UserCases.Create.Command;
using AuthorizationOperation.Application.UserCases.Create.ViewModels;
using AuthorizationOperation.Application.UserCases.FindAll.Queries;
using AuthorizationOperation.Application.UserCases.FindAll.ViewModels;
using AuthorizationOperation.Application.UserCases.FindOne.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        /// 
        /// </summary>
        /// <remarks>This version 2.0 is the current one</remarks>
        /// <returns></returns>
        [HttpGet("{uuid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorizationResponse))]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public async Task<IActionResult> GetOne(Guid uuid)
        {
            var response = await this.mediator.Send(new AuthorizationGetQuery() { UUID = uuid });
            if(response == null) return this.NotFound();
            return this.Ok(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>This method exists only in the current version, to show how the same "Controller" can evolve and maintain the versioning</remarks>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorizationPageResponse))]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public async Task<IActionResult> GetAll([FromQuery] List<EnumStatusRequest> statusIn, string sort, ushort offset, ushort limit)
        {
            var criteria = new AuthorizationCriteriaRequest()
            {
                listStatus = statusIn
            };

            var response = await this.mediator.Send(new AuthorizationGetAllQuery()
            {
                Criteria = criteria,
                Limit = limit,
                Offset = offset,
                Sort = sort
            });
            return this.Ok(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>This method exists only in the current version, to show how the same "Controller" can evolve and maintain the versioning</remarks>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AuthorizationResponse))]
        public async Task<IActionResult> Create([FromBody] CreateAuthorizationRequest req)
        {
            var response = await this.mediator.Send(new CreateAuthorizationCommand()
            {
                Request = req
            });
            return this.Ok(response);
        }
    }
}
