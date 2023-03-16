using GT.Trace.App.UseCases.Lines.GetNewSubAssemblyID;
using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GT.Trace.UI.MaterialLoadingWebApi.EndPoints.Lines.CreateSubAssemblyEti
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateSubAssemblyEtiController : ControllerBase
    {
        private readonly ILogger<CreateSubAssemblyEtiController> _logger;

        private readonly IMediator _mediator;

        private readonly ResultViewModel<CreateSubAssemblyEtiController> _viewModel;

        public CreateSubAssemblyEtiController(ILogger<CreateSubAssemblyEtiController> logger, IMediator mediator, ResultViewModel<CreateSubAssemblyEtiController> viewModel)
        {
            _logger = logger;
            _mediator = mediator;
            _viewModel = viewModel;
        }

        [HttpPost]
        [Route("/api/lines/{lineCode}/subassemblies")]
        public async Task<IActionResult> ExecuteAsync([FromRoute] string lineCode)
        {
            if (!GetNewSubAssemblyIDRequest.CanCreate(lineCode, out var errors))
            {
                return BadRequest(errors.Select(e => $"- {e}").Aggregate((a, b) => $"{a}\n{b}"));
            }
            try
            {
                _ = await _mediator.Send(GetNewSubAssemblyIDRequest.Create(lineCode)).ConfigureAwait(false);
                return _viewModel.IsSuccess ? Ok(_viewModel) : StatusCode(500, _viewModel);
            }
            catch (Exception ex)
            {
                var innermostException = ex;
                while (innermostException.InnerException != null) innermostException = innermostException.InnerException!;
                return StatusCode(500, new { IsSuccess = false, innermostException.Message, UtcTimeStamp = DateTime.UtcNow });
            }
        }
    }
}