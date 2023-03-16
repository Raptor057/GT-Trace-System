using Azure.Core;
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.UseCases.Auth;
using GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.GetLineStatus;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        //private readonly ILogger<AuthController> _logger;
        private readonly IMediator _mediator;
        //private readonly GenericViewModel<AuthController> _viewModel;

        public AuthController(/*ILogger<AuthController> logger, */IMediator mediator/*, GenericViewModel<AuthController> viewModel*/)
        {
            //_logger = logger;
            _mediator = mediator;
            //_viewModel = viewModel;
        }

        [HttpGet]
        [Route("/api/Auth/{AuthorizedUserPassword}")]
        public async Task<IActionResult> Execute([FromRoute] string AuthorizedUserPassword)

        {
            var request = new AuthRequest(AuthorizedUserPassword);
            var UserPassword = await _mediator.Send(request).ConfigureAwait(false);
            return Ok(UserPassword);
            //try
            //{
            //    _ = await _mediator.Send(request).ConfigureAwait(false);
            //    if (_viewModel is IFailure failure)
            //    {
            //        return StatusCode(500, _viewModel.Fail(failure.Message));
            //    }
            //    return Ok(_viewModel);
            //}
            //catch (Exception ex)
            //{
            //    var innerEx = ex;
            //    while (innerEx.InnerException != null) innerEx = innerEx.InnerException;
            //    return StatusCode(500, _viewModel.Fail(innerEx.Message));
            //}
        }
    }
}
