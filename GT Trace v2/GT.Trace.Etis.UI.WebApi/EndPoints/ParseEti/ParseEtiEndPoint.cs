using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.Etis.App.UseCases.ParseEti;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.Etis.UI.WebApi.EndPoints.ParseEti
{
    [ApiController]
    public class ParseEtiEndPoint : ControllerBase
    {
        private readonly ILogger<ParseEtiEndPoint> _logger;

        private readonly IMediator _mediator;

        private readonly ResultViewModel<ParseEtiEndPoint> _viewModel;

        public ParseEtiEndPoint(ILogger<ParseEtiEndPoint> logger, IMediator mediator, ResultViewModel<ParseEtiEndPoint> viewModel)
        {
            _logger = logger;
            _mediator = mediator;
            _viewModel = viewModel;
        }

        [HttpGet]
        [Route("/api/parse/{input}")]
        public async Task<IActionResult> Execute([FromRoute] string input)
        {
            var request = new ParseEtiRequest(input);
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