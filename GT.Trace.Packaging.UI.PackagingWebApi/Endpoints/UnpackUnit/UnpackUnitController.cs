using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.UseCases.UnpackUnit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.UnpackUnit
{
    [ApiController]
    [Route("[controller]")]
    public class UnpackUnitController : ControllerBase
    {
        private readonly ILogger<UnpackUnitController> _logger;

        private readonly IMediator _mediator;

        private readonly GenericViewModel<UnpackUnitController> _viewModel;

        public UnpackUnitController(ILogger<UnpackUnitController> logger, IMediator mediator, GenericViewModel<UnpackUnitController> viewModel)
        {
            _logger = logger;
            _mediator = mediator;
            _viewModel = viewModel;
        }

        [HttpDelete]
        [Route("/api/lines/{lineName}/container")]
        public async Task<IActionResult> Execute([FromRoute] string lineName, [FromBody] UnpackUnitRequestBody requestBody)
        {
            var request = new UnpackUnitRequest(lineName, requestBody.ScannerInput ?? "", requestBody.WorkOrderCode ?? "", requestBody.LineCode ?? "");
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