using GT.Trace.Changeover.App.UseCases.GetGamma;
using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.Changeover.UI.HttpApi.EndPoints.Lines.GetGamma
{
    [ApiController]
    public class GetGammaEndPoint : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly GenericViewModel<GetGammaEndPoint> _model;

        private readonly ILogger<GetGammaEndPoint> _logger;

        public GetGammaEndPoint(ILogger<GetGammaEndPoint> logger, IMediator mediator, GenericViewModel<GetGammaEndPoint> model)
        {
            _logger = logger;
            _mediator = mediator;
            _model = model;
        }

        [HttpGet]
        [Route("api/lines/{lineCode}/gamma/{partNo}")]
        public async Task<IActionResult> Execute([FromRoute] string lineCode, [FromRoute] string partNo)
        {
            try
            {
                _ = await _mediator.Send(new GetGammaRequest(lineCode, partNo, lineCode)).ConfigureAwait(false);
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