using GT.Trace.App.UseCases.Lines.GetWorkOrder;
using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.UI.CommonWebApi.EndPoints.Lines.GetWorkOrder
{
    [ApiController]
    public class GetWorkOrderController : ControllerBase
    {
        private readonly ILogger<GetWorkOrderController> _logger;

        private readonly ResultViewModel<GetWorkOrderController> _viewModel;

        private readonly IMediator _mediator;

        public GetWorkOrderController(ILogger<GetWorkOrderController> logger, ResultViewModel<GetWorkOrderController> viewModel, IMediator mediator)
        {
            _logger = logger;
            _viewModel = viewModel;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("api/lines/{lineCode}/workorder")]
        public async Task<IActionResult> Execute([FromRoute] string lineCode)
        {
            var request = new GetWorkOrderRequest(lineCode);
            try
            {
                _ = await _mediator.Send(request).ConfigureAwait(false);
                return _viewModel.IsSuccess ? Ok(_viewModel) : StatusCode(500, _viewModel);
            }
            catch (Exception ex)
            {
                var exception = ex;
                while (exception.InnerException != null) exception = ex.InnerException!;
                return StatusCode(500, _viewModel.Fail(exception.Message));
            }
        }
    }
}