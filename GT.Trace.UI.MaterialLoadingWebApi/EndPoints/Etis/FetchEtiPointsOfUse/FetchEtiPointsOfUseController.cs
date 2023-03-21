using GT.Trace.App.UseCases.MaterialLoading.FetchEtiPointsOfUse;
using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.UI.MaterialLoadingWebApi.EndPoints.PointsOfUse.FetchEtiPointsOfUse
{
    [ApiController]
    [Route("[controller]")]
    public partial class FetchEtiPointsOfUseController : ControllerBase
    {
        private readonly ILogger<FetchEtiPointsOfUseController> _logger;

        private readonly IMediator _mediator;

        private readonly ResultViewModel<FetchEtiPointsOfUseController> _viewModel;

        public FetchEtiPointsOfUseController(ResultViewModel<FetchEtiPointsOfUseController> viewModel, ILogger<FetchEtiPointsOfUseController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
            _viewModel = viewModel;
        }

        [HttpGet]
        [Route("/api/etis/{etiNo}/pointsofuse")]
        public async Task<IActionResult> ExecuteAsync([FromRoute] string etiNo, [FromQuery] string lineCode, [FromQuery] string partNo)
        {
            try
            {
                if (!FetchEtiPointsOfUseRequest.CanCreate(lineCode, partNo, etiNo, out var errors))
                {
                    return StatusCode(500, new { Message = errors.Select(e => $"- {e}").Aggregate((a, b) => $"{a}\n{b}"), IsSuccess = false, UtcTimeStamp = DateTime.UtcNow });
                }

                var result = await _mediator.Send(FetchEtiPointsOfUseRequest.Create(lineCode, partNo, etiNo)).ConfigureAwait(false);
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