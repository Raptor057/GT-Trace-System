using GT.Trace.BomSnapShot.App.UseCases.GetSeqSnapshotID;
using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.BomSnapShotWebApi.Endpoints.SeqSnapshotID
{
    [ApiController]
    [Route("[controller]")]

    public class GetSeqSnapshotIDController:ControllerBase
    {
        private readonly ILogger<GetSeqSnapshotIDController> _logger;
        private readonly IMediator _mediator;
        private readonly GenericViewModel<GetSeqSnapshotIDController> _viewModel;

        public GetSeqSnapshotIDController(ILogger<GetSeqSnapshotIDController> logger, IMediator mediator, GenericViewModel<GetSeqSnapshotIDController> viewModel)
        {
            _logger=logger;
            _mediator=mediator;
            _viewModel=viewModel;
        }
        [HttpGet]
        [Route("/api/lines/getseqsnapshotid/{linecode}/{partNo}")]
        public async Task<IActionResult> Get([FromRoute] string linecode, [FromRoute] string partNo)
        {
            var request = new GetSeqSnapshotIDRequest(linecode,partNo);
            try
            {
                _ = await _mediator.Send(request).ConfigureAwait(false);
                return _viewModel.IsSuccess ? Ok(_viewModel) : StatusCode(500, _viewModel);
            }
            catch (Exception ex)
            {
                var innerEx = ex;
                while (innerEx.InnerException != null) innerEx = innerEx.InnerException!;
                return StatusCode(500, _viewModel.Fail(innerEx.Message));
            }
        }
    }
}
