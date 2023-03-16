using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.Etis.App.UseCases.GetEti;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.Etis.UI.WebApi.EndPoints.GetEti
{
    [ApiController]
    public class GetEtiEndPoint : ControllerBase
    {
        private readonly ILogger<GetEtiEndPoint> _logger;

        private readonly IMediator _mediator;

        private readonly ResultViewModel<GetEtiEndPoint> _viewModel;

        public GetEtiEndPoint(ILogger<GetEtiEndPoint> logger, IMediator mediator, ResultViewModel<GetEtiEndPoint> viewModel)
        {
            _logger = logger;
            _mediator = mediator;
            _viewModel = viewModel;
        }

        [HttpGet]
        [Route("/api/info/{etiID}/{etiNo}")]
        public async Task<IActionResult> Execute([FromRoute] long etiID, [FromRoute] string etiNo)
        {
            var request = new GetEtiRequest(etiID, etiNo);
            try
            {
                _ = await _mediator.Send(request).ConfigureAwait(false);
                if (_viewModel is IFailure failure)
                {
                    return StatusCode(500, _viewModel.Fail(failure.Message));
                }
                return Ok(_viewModel);
            }
            catch (Exception ex)
            {
                var innerEx = ex;
                while (innerEx.InnerException != null) innerEx = innerEx.InnerException;
                return StatusCode(500, _viewModel.Fail(innerEx.Message));
            }
        }
    }
}