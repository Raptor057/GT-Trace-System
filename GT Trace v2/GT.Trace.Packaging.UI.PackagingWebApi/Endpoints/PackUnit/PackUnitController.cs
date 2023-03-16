using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.UseCases.PackUnit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.PackUnit
{
    [ApiController]
    [Route("[controller]")]
    public class PackUnitController : ControllerBase
    {
        private readonly ILogger<PackUnitController> _logger;

        private readonly IMediator _mediator;

        private readonly GenericViewModel<PackUnitController> _viewModel;

        public PackUnitController(ILogger<PackUnitController> logger, IMediator mediator, GenericViewModel<PackUnitController> viewModel)
        {
            _logger = logger;
            _mediator = mediator;
            _viewModel = viewModel;
        }

        [HttpPost]
        [Route("/api/lines/{hostname}/container")]
        public async Task<IActionResult> Execute([FromRoute] string hostname, [FromBody] PackUnitRequestBody requestBody)
        {
            if (!PackUnitRequest.CanCreate(requestBody.ScannerInput, hostname, out var errors))
            {
                return StatusCode(400, _viewModel.Fail(errors.ToString()));
            }

            var request = PackUnitRequest.Create(requestBody.ScannerInput, hostname, requestBody.PalletSize, requestBody.ContainerSize, requestBody.LineCode, requestBody.PoNumber);
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