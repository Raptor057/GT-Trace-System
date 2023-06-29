using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.UseCases.JoinEZMotors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.JoinEZMotors
{
    [ApiController]
    [Route("[controller]")]
    public class JoinEZMotorsController:ControllerBase
    {
        private readonly ILogger<JoinEZMotorsController> _logger;
        private readonly IMediator _mediator;
        private readonly GenericViewModel<JoinEZMotorsController> _viewModel;

        public JoinEZMotorsController(ILogger<JoinEZMotorsController> logger, IMediator mediator, GenericViewModel<JoinEZMotorsController> viewModel)
        {
            _logger=logger;
            _mediator=mediator;
            _viewModel=viewModel;
        }

        [HttpPost]
        [Route("/api/JoinEZMotors/")]
        public async Task<IActionResult> Execute([FromBody] JoinEZMotorsRequestBody requestBody)
        {
            var request = JoinEZMotorsRequest.Create(requestBody.ScannerInputUnitID ?? "", requestBody.ScannerOutputMotorID1 ?? "", requestBody.ScannerOutputMotorID2 ?? "", requestBody.isEnable);
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
