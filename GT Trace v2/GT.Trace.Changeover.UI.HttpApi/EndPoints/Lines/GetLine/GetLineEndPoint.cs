using GT.Trace.Changeover.App.UseCases.GetLine;
using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.Changeover.UI.HttpApi.EndPoints.Lines.GetLine
{
    [ApiController]
    public class GetLineEndPoint : ControllerBase
    {
        private readonly ILogger<GetLineEndPoint> _logger;

        private readonly IMediator _mediator;

        private readonly GenericViewModel<GetLineEndPoint> _model;

        public GetLineEndPoint(ILogger<GetLineEndPoint> logger, IMediator mediator, GenericViewModel<GetLineEndPoint> model)
        {
            _logger = logger;
            _mediator = mediator;
            _model = model;
        }

        [HttpGet]
        [Route("api/lines/{lineCode}")]
        public async Task<IActionResult> Execute([FromRoute] string lineCode)
        {
            try
            {
                _ = await _mediator.Send(new GetLineByCodeRequest(lineCode)).ConfigureAwait(false);
                return _model.IsSuccess ? Ok(_model) : StatusCode(500, _model);
            }
            catch (Exception ex)
            {
                var innerEx = ex;
                while (innerEx.InnerException != null) innerEx = innerEx.InnerException!;
                return StatusCode(500, _model.Fail(innerEx.Message));
            }
        }
    }
}