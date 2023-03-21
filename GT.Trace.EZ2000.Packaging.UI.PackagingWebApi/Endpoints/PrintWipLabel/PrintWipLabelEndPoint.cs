using GT.Trace.Common.CleanArch;
using GT.Trace.EZ2000.Packaging.App.UseCases.PrintWipLabel;
using GT.Trace.EZ2000.Packaging.UI.PackagingWebApi.Hubs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.PrintWipLabel
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrintWipLabelEndPoint : ControllerBase
    {
        private readonly ILogger<PrintWipLabelEndPoint> _logger;

        private readonly IMediator _mediator;

        private readonly GenericViewModel<PrintWipLabelEndPoint> _viewModel;

        public PrintWipLabelEndPoint(ILogger<PrintWipLabelEndPoint> logger, GenericViewModel<PrintWipLabelEndPoint> viewModel, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
            _viewModel = viewModel;
        }

        [HttpPost]
        [Route("/api/lines/{hostname}/container/wip")]
        public async Task<IActionResult> Execute([FromRoute] string hostname)
        {
            var request = new PrintWipLabelRequest(hostname);
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