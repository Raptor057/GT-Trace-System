using GT.Trace.Changeover.App.UseCases.GetWorkOrder;
using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GT.Trace.Changeover.UI.HttpApi.EndPoints.WorkOrders.GetWorkOrderByLineID
{
    [ApiController]
    public class GetWorkOrderByLineIDEndPoint : ControllerBase
    {
        private readonly ILogger<GetWorkOrderByLineIDEndPoint> _logger;

        private readonly IMediator _mediator;

        private readonly GenericViewModel<GetWorkOrderByLineIDEndPoint> _model;

        public GetWorkOrderByLineIDEndPoint(ILogger<GetWorkOrderByLineIDEndPoint> logger, IMediator mediator, GenericViewModel<GetWorkOrderByLineIDEndPoint> model)
        {
            _logger = logger;
            _mediator = mediator;
            _model = model;
        }

        [HttpGet]
        [Route("api/workorders")]
        public async Task<IActionResult> Execute([FromQuery] uint? lineID)
        {
            try
            {
                if (!lineID.HasValue)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { Success = false, Message = "El identificador de la línea no se encontró." });
                }
                _ = await _mediator.Send(new GetWorkOrderByLineIDRequest((int)lineID.Value)).ConfigureAwait(false);
                return _model.IsSuccess ? Ok(_model) : StatusCode(500, _model);
            }
            catch (Exception ex)
            {
                var innerEx = ex;
                while (innerEx.InnerException != null) innerEx = innerEx.InnerException!;
                return StatusCode(500, _model.Fail(innerEx.Message));
            }
        }
    }
}