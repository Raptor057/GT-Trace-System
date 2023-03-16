using GT.Trace.App.UseCases.MaterialLoading.FetchPointOfUseLines;
using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.UI.MaterialLoadingWebApi.EndPoints.PointsOfUse.GetPointOfUseLines
{
    [ApiController]
    [Route("[controller]")]
    public partial class GetPointOfUseLinesController : ControllerBase
    {
        private readonly ILogger<GetPointOfUseLinesController> _logger;

        private readonly IMediator _mediator;

        private readonly ResultViewModel<GetPointOfUseLinesController> _viewModel;

        public GetPointOfUseLinesController(ResultViewModel<GetPointOfUseLinesController> viewModel, ILogger<GetPointOfUseLinesController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
            _viewModel = viewModel;
        }

        [HttpGet]
        [Route("/api/pointsofuse/{pointOfUseCode}/lines")]
        public async Task<IActionResult> FetchPointOfUseLines([FromRoute] string pointOfUseCode)
        {
            try
            {
                if (!FetchPointOfUseLinesRequest.CanCreate(pointOfUseCode, out var errors))
                {
                    return StatusCode(400, _viewModel.Fail(errors.Select(e => $"- {e}").Aggregate((a, b) => $"{a}\n{b}")));
                }

                await _mediator.Send(FetchPointOfUseLinesRequest.Create(pointOfUseCode)).ConfigureAwait(false);
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