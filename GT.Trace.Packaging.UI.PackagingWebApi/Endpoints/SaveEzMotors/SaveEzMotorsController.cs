using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.UseCases.SaveEzMotors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.SaveEzMotors
{
    [ApiController]
    [Route("[controller]")]
    public class SaveEzMotorsController : ControllerBase
    {
        private readonly ILogger<SaveEzMotorsController> _logger;
        private readonly IMediator _mediator;
        private readonly GenericViewModel<SaveEzMotorsController> _viewModel;

        public SaveEzMotorsController( ILogger<SaveEzMotorsController> logger, IMediator mediator, GenericViewModel<SaveEzMotorsController> viewModel)
        {
            _logger=logger;
            _mediator=mediator;
            _viewModel=viewModel;
        }
        [HttpPost]
        [Route ("/api/SaveEZMotorsPinion/")]
        public async Task<IActionResult> Execute([FromBody] SaveEzMotorsRequestBody requestBody)
        {
            var request = SaveEzMotorsRequest.Create(requestBody.Model ?? "", requestBody.ScannerInputEzQR ?? "", requestBody.LineCode ?? "");
            try
            {
                _ = await _mediator.Send(request).ConfigureAwait(false);
                return _viewModel.IsSuccess ? Ok(_viewModel) : StatusCode(500, _viewModel);
            }
            catch (Exception ex)
            {
                var innerEx = ex;
                while (innerEx.InnerException != null) innerEx = innerEx.InnerException;
                return StatusCode(500, _viewModel.Fail(innerEx.Message ?? ""));
            }
        }
    }
}
