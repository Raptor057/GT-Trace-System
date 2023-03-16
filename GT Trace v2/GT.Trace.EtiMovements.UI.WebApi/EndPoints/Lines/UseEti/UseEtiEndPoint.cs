using GT.Trace.Common.CleanArch;
using GT.Trace.EtiMovements.App.UseCases.UseEti;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.EtiMovements.UI.WebApi.EndPoints.Lines.UseEti
{
    public class UseEtiEndPoint : ApiControllerBase<UseEtiEndPoint>
    {
        public UseEtiEndPoint(IMediator mediator, ResultViewModel<UseEtiEndPoint> viewModel, ILogger<UseEtiEndPoint> logger)
            : base(mediator, viewModel, logger)
        { }

        [HttpPut]
        [Route("/api/lines/{lineCode}/etis")]
        public async Task<IActionResult> Execute([FromRoute] string lineCode, [FromBody] UseEtiRequestBody requestBody)
        {
            if (!UseEtiRequest.CanCreate(lineCode, requestBody.EtiInput, out var errors))
            {
                return BadRequest(ViewModel.Fail(errors.ToString()));
            }
            return await ExecuteAsync(UseEtiRequest.Create(lineCode, requestBody.EtiInput)).ConfigureAwait(false);
        }
    }
}