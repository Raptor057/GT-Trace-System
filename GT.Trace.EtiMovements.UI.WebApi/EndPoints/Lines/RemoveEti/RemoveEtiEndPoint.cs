using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.EtiMovements.App.UseCases.ReturnEti;
using GT.Trace.EtiMovements.App.UseCases.UnloadEti;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.EtiMovements.UI.WebApi.EndPoints.Lines.RemoveEti
{
    public class RemoveEtiEndPoint : ApiControllerBase<RemoveEtiEndPoint>
    {
        public RemoveEtiEndPoint(IMediator mediator, ResultViewModel<RemoveEtiEndPoint> viewModel, ILogger<RemoveEtiEndPoint> logger)
            : base(mediator, viewModel, logger)
        { }

        [HttpDelete]
        [Route("/api/lines/{lineCode}/etis")]
        public async Task<IActionResult> Execute([FromRoute] string lineCode, [FromBody] RemoveEtiRequestBody requestBody, [FromQuery] int? isChangeOver)
        {
            ErrorList errors;
            if (requestBody.IsReturn)
            {
                if (!ReturnEtiRequest.CanCreate(lineCode, requestBody.EtiInput, out errors))
                {
                    return BadRequest(ViewModel.Fail(errors.ToString()));
                }
                return await ExecuteAsync(ReturnEtiRequest.Create(lineCode, requestBody.EtiInput, (isChangeOver ?? 0) == 1));
            }

            if (!UnloadEtiRequest.CanCreate(lineCode, requestBody.EtiInput, out errors))
            {
                return BadRequest(ViewModel.Fail(errors.ToString()));
            }
            return await ExecuteAsync(UnloadEtiRequest.Create(lineCode, requestBody.EtiInput));
        }
    }
}