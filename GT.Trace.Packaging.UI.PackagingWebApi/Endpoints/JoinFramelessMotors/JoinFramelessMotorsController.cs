using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.UseCases.JoinFramelessMotors;
using GT.Trace.Packaging.App.UseCases.PackUnit;
using GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.SetLineHeadcount;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.JoinFramelessMotors
{
    [ApiController]
    [Route("[controller]")]
    public class JoinFramelessMotorsController : ControllerBase
    {
        private readonly ILogger<JoinFramelessMotorsController> _logger;
        private readonly IMediator _mediator;
        private readonly GenericViewModel<JoinFramelessMotorsController> _viewModel;

        public JoinFramelessMotorsController(ILogger<JoinFramelessMotorsController> logger, IMediator mediator, GenericViewModel<JoinFramelessMotorsController> viewModel)
        {
            _logger = logger;
            _mediator = mediator;
            _viewModel = viewModel;
        }

        [HttpPost]
        [Route("/api/JoinFramelessMotors/")]
        public async Task<IActionResult> Execute([FromBody] JoinFramelessMotorsRequestBody requestBody)
        {
            var request = JoinFramelessMotorsRequest.Create(requestBody.ScannerInputUnitID ?? "", requestBody.ScannerInputComponentID ?? "", requestBody.LineCode ?? "" , requestBody.PartNo ?? "");
            try
            {
                _ = await _mediator.Send(request).ConfigureAwait(false);
                return _viewModel.IsSuccess ? Ok(_viewModel) : StatusCode(500, _viewModel);
            }
            catch (Exception ex)
            {
                var innerEx = ex;
                while (innerEx.InnerException != null) innerEx = innerEx.InnerException!;
                return StatusCode(500, _viewModel.Fail(innerEx.Message ?? ""));
            }
        }

    }
}
