using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.UseCases.JoinLinePallet;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.JoinPalletQR
{
    [ApiController]
    [Route("[controller]")]
    public class JoinLinePalletController : ControllerBase
    {
        private readonly ILogger<JoinLinePalletController> _logger;
        private readonly IMediator _mediator;
        private readonly GenericViewModel<JoinLinePalletController> _viewModel;

        public JoinLinePalletController(ILogger<JoinLinePalletController> logger, IMediator mediator, GenericViewModel<JoinLinePalletController> viewModel)
        {
            _logger=logger;
            _mediator=mediator;
            _viewModel=viewModel;
        }
        [HttpPost]
        [Route("/api/PalletQR")]
        public async Task<IActionResult> Execute([FromBody] JoinLinePalletRequestBody requestBody)
        {
            var request = JoinLinePalletRequest.Create(requestBody.ScannerInputUnitID ?? "", requestBody.ScannerInputPalletID ?? "", requestBody.LineCode ?? "", requestBody.IsEnable);
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
