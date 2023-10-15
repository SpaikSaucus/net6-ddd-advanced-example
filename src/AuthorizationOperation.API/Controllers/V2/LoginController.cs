using AuthorizationOperation.API.ViewModels;
using AuthorizationOperation.Application.UserCases.Login.Commands;
using AuthorizationOperation.Infrastructure.Services.Accessor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthorizationOperation.API.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICurrentUserAccessor currentUser;

        public LoginController(IMediator mediator, ICurrentUserAccessor currentUser)
        {
            this.mediator = mediator;
            this.currentUser = currentUser;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest req)
        {
            var cmd = new UserLoginCommand()
            {
                UserName = req.UserName,
                Password = req.Password
            };
            var result = await mediator.Send(cmd);
            return Ok(result);
        }
    }
}
