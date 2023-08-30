using Azure.Core;
using GT.Trace.App.UseCases.Lines.UpdateGama;
using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.UI.CommonWebApi.EndPoints.Lines.UpdateBomLine
{
    [ApiController]
    public class UpdateBomLineEndPoint : ControllerBase
    {
        private readonly ILogger<UpdateBomLineEndPoint> _logger;
        private readonly IMediator _mediator;
        private readonly GenericViewModel<UpdateBomLineEndPoint> _model;

        public UpdateBomLineEndPoint(ILogger<UpdateBomLineEndPoint> logger, IMediator mediator, GenericViewModel<UpdateBomLineEndPoint> model)
        {
            _logger=logger;
            _mediator = mediator;
            _model=model;
        }

        [HttpPut]
        [Route("api/lines/updategama/partno/{partNo}/lineCode/{lineCode}")]
        public async Task<IActionResult> Execute([FromRoute] string partNo, [FromRoute] string lineCode)
        {
            var request = new UpdateBomLineRequest(partNo, lineCode);
            try
            {
                _ = await _mediator.Send(request).ConfigureAwait(false);
                return _model.IsSuccess ? Ok(_model) : StatusCode(500, _model);
            }
            catch (Exception ex)
            {
                var innerEx = ex;
                while (innerEx.InnerException != null) innerEx = innerEx.InnerException;
                return Ok(new { IsSuccess = false, Message = innerEx.Message });
            }
        }
    }
}
