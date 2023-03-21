using GT.Trace.Common.CleanArch;
using GT.Trace.EZ2000.Packaging.App.UseCases.SetLineHeadcount;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.SetLineHeadcount
{
    [ApiController]
    [Route("[controller]")]
    public class SetLineHeadcountController : ControllerBase
    {
        private readonly ILogger<SetLineHeadcountController> _logger;

        private readonly IMediator _mediator;

        private readonly GenericViewModel<SetLineHeadcountController> _viewModel;

        public SetLineHeadcountController(ILogger<SetLineHeadcountController> logger, IMediator mediator, GenericViewModel<SetLineHeadcountController> viewModel)
        {
            _logger = logger;
            _mediator = mediator;
            _viewModel = viewModel;
        }

        [HttpPut]
        [Route("/api/lines/{lineCode}/workorders/{workOrderCode}/headcount/{value}")]
        public async Task<IActionResult> Get([FromRoute] string lineCode, [FromRoute] string workOrderCode, [FromRoute] int value)
        {
            var request = new SetLineHeadcountRequest(lineCode, workOrderCode, value);
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