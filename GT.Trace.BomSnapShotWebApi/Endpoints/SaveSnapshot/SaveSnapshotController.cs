using MediatR;
using Microsoft.AspNetCore.Mvc;
using GT.Trace.Common.CleanArch;
using GT.Trace.BomSnapShot.App.UseCases.SaveSnapshot;
using System.ComponentModel;

namespace GT.Trace.BomSnapShotWebApi.Endpoints.SaveSnapshot
{
    [ApiController]
    [Route("[controller]")]
    public class SaveSnapshotController : ControllerBase
    {
        private readonly ILogger<SaveSnapshotController> _logger;
        private readonly IMediator _mediator;
        private readonly GenericViewModel<SaveSnapshotController> _viewModel;

        public SaveSnapshotController(ILogger<SaveSnapshotController> logger, IMediator mediator, GenericViewModel<SaveSnapshotController> viewModel)
        {
            _logger = logger;
            _mediator=mediator;
            _viewModel=viewModel;
        }

        //[HttpPost]
        [HttpPut]
        [Route("/api/SaveSnapshot/{pointOfUseCode}/{componentNo}")]
        public async Task<IActionResult> Execute([FromRoute] string pointOfUseCode, [FromRoute] string componentNo)
        {
            var request = SaveSnapshotRequest.Create(pointOfUseCode ?? "", componentNo ?? "");
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
