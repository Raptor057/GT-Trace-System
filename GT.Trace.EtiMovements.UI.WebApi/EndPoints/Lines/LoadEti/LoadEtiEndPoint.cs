using GT.Trace.Common.CleanArch;
using GT.Trace.EtiMovements.App.UseCases.LoadEti;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.EtiMovements.UI.WebApi.EndPoints.Lines.LoadEti
{
    public class LoadEtiEndPoint : ApiControllerBase<LoadEtiEndPoint>
    {
        public LoadEtiEndPoint(IMediator mediator, ResultViewModel<LoadEtiEndPoint> viewModel, ILogger<LoadEtiEndPoint> logger)
            : base(mediator, viewModel, logger)
        { }

        [HttpPost]
        [Route("/api/lines/{lineCode}/etis")]
        public async Task<IActionResult> Execute([FromRoute] string lineCode, [FromBody] LoadEtiRequestBody requestBody)
        {
            if (!LoadEtiRequest.CanCreate(lineCode, requestBody.PointOfUseCode, requestBody.EtiInput, out var errors))
            {
                return BadRequest(ViewModel.Fail(errors.ToString()));
            }
            return await ExecuteAsync(LoadEtiRequest.Create(lineCode, requestBody.PointOfUseCode, requestBody.EtiInput, requestBody.IgnoreCapacity)).ConfigureAwait(false);
        }
    }
}