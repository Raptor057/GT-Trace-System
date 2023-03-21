using GT.Trace.Common.CleanArch;
using GT.Trace.EZ2000.Packaging.App.UseCases.UpdateActiveEti;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.EZ2000.Packaging.UI.PackagingWebApi.Endpoints.UpdateActiveEti
{

        [ApiController]
        [Route("[controller]")]
        public class UpdateActiveEtiController : ControllerBase
        {
            private readonly ILogger<UpdateActiveEtiController> _logger;
            private readonly IMediator _mediator;
            private readonly GenericViewModel<UpdateActiveEtiController> _viewModel;

            public UpdateActiveEtiController(ILogger<UpdateActiveEtiController> logger, IMediator mediator, GenericViewModel<UpdateActiveEtiController> viewModel)
            {
                _logger = logger;
                _mediator = mediator;
                _viewModel = viewModel;
            }

            [HttpPut]
            [Route("/api/eti/{etiNo}/updateetitraza")]
            public async Task<IActionResult> Get([FromRoute] string etiNo)
            {
                var request = new UpdateEtiTrazaRequest(etiNo);
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
