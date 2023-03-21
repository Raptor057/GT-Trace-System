using GT.Trace.App.UseCases.MaterialLoading.FetchPointOfUseEtis;
using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.UI.MaterialLoadingWebApi.EndPoints.PointsOfUse.GetPointOfUseEtis
{
    [ApiController]
    [Route("[controller]")]
    public partial class GetPointOfUseEtisController : ControllerBase
    {
        private readonly ILogger<GetPointOfUseEtisController> _logger;

        private readonly IMediator _mediator;

        private readonly ResultViewModel<GetPointOfUseEtisController> _viewModel;

        public GetPointOfUseEtisController(ResultViewModel<GetPointOfUseEtisController> viewModel, ILogger<GetPointOfUseEtisController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
            _viewModel = viewModel;
        }

        [HttpGet]
        [Route("/api/pointsofuse/{pointOfUseCode}/etis")]
        public async Task<IActionResult> ExecuteAsync([FromRoute] string pointOfUseCode)
        {
            try
            {
                //if (string.IsNullOrWhiteSpace(pointOfUseCode)) return StatusCode(400, HttpApiResponse.Fail("El código de túnel es obligatorio y se encuentra en blanco."));
                var result = await _mediator.Send(new FetchPointOfUseEtisRequest(pointOfUseCode)).ConfigureAwait(false);
                return _viewModel.IsSuccess ? Ok(_viewModel) : StatusCode(500, _viewModel);
            }
            catch (Exception ex)
            {
                var innermostEx = ex;
                while (innermostEx.InnerException != null) innermostEx = innermostEx.InnerException!;
                return StatusCode(500, new { innermostEx.Message, IsSuccess = false, UtcTimeStamp = DateTime.UtcNow });
            }
        }
    }
}