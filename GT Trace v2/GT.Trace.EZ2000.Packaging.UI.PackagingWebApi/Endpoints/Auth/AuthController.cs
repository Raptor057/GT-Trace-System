using GT.Trace.EZ2000.Packaging.App.UseCases.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.EZ2000.Packaging.UI.PackagingWebApi.Endpoints.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/api/Auth/{password}")]
        public async Task<IActionResult> Execute(string password)
        {
            var request = new AuthRequest(password);
            var UserPassword = await _mediator.Send(request).ConfigureAwait(false);
            return Ok(UserPassword);

        }
    }
}
