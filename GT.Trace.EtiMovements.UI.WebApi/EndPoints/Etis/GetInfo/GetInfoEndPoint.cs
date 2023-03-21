using GT.Trace.Common.CleanArch;
using GT.Trace.EtiMovements.App.UseCases.GetEtiInfo;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.EtiMovements.UI.WebApi.EndPoints.Etis.GetInfo
{
    public class GetInfoEndPoint : ApiControllerBase<GetInfoEndPoint>
    {
        public GetInfoEndPoint(IMediator mediator, ResultViewModel<GetInfoEndPoint> viewModel, ILogger<GetInfoEndPoint> logger)
            : base(mediator, viewModel, logger)
        { }

        [HttpGet]
        [Route("/api/etis/{etiNo}")]
        public async Task<IActionResult> Execute([FromRoute] string etiNo)
        {
            return await ExecuteAsync(new GetEtiInfoRequest(etiNo)).ConfigureAwait(false);
        }
    }
}