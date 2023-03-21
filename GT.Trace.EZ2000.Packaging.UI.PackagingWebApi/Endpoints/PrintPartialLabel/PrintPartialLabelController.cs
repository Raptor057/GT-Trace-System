using GT.Trace.Common.CleanArch;
using GT.Trace.EZ2000.Packaging.App.UseCases.PrintPartialLabel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.PrintPartialLabel
{
    [ApiController]
    [Route("[controller]")]
    public class PrintPartialLabelController : ControllerBase
    {
        private readonly ILogger<PrintPartialLabelController> _logger;

        private readonly IMediator _mediator;

        private readonly GenericViewModel<PrintPartialLabelController> _viewModel;

        public PrintPartialLabelController(ILogger<PrintPartialLabelController> logger, IMediator mediator, GenericViewModel<PrintPartialLabelController> viewModel)
        {
            _logger = logger;
            _mediator = mediator;
            _viewModel = viewModel;
        }

        [HttpPost]
        [Route("/api/lines/{hostname}/container/partial")]
        public async Task<IActionResult> Execute([FromRoute] string hostname)
        {
            var request = new PrintPartialLabelRequest(hostname);
            try
            {
                _ = await _mediator.Send(request).ConfigureAwait(false);
                return _viewModel.IsSuccess ? Ok(_viewModel) : StatusCode(500, _viewModel);
            }
            catch (Exception ex)
            {
                var innerEx = ex;
                while (innerEx.InnerException != null) innerEx = innerEx.InnerException!;
                return StatusCode(500, _viewModel.Fail(innerEx.Message));
            }
        }
    }
}