using GT.Trace.Common.CleanArch;
using GT.Trace.Etis.App.UseCases.GetEtiInfo;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.Etis.UI.WebApi.EndPoints
{
    [ApiController]
    public class GetEtiInfoController : ControllerBase
    {
        private readonly ILogger<GetEtiInfoController> _logger;

        private readonly IMediator _mediator;

        private readonly ResultViewModel<GetEtiInfoController> _viewModel;

        public GetEtiInfoController(ILogger<GetEtiInfoController> logger, IMediator mediator, ResultViewModel<GetEtiInfoController> viewModel)
        {
            _logger = logger;
            _mediator = mediator;
            _viewModel = viewModel;
        }

        [HttpGet]
        [Route("/api/info/{input}")]
        public async Task<IActionResult> Execute([FromRoute] string input)
        {
            if (!GetEtiInfoRequest.CanCreate(input, out var errors))
            {
                return BadRequest(_viewModel.Fail(errors.Select(err => $"- {err}").Aggregate((x, y) => $"{x}\n{y}")));
            }

            var request = GetEtiInfoRequest.Create(input);
            try
            {
                _ = await _mediator.Send(request).ConfigureAwait(false);
                return _viewModel.IsSuccess ? Ok(_viewModel) : StatusCode(500, _viewModel);
            }
            catch (Exception ex)
            {
                var innermostException = ex;
                while (innermostException.InnerException != null) innermostException = ex.InnerException!;
                return StatusCode(500, _viewModel.Fail(innermostException.Message));
            }
        }

        [HttpPost]
        [Route("/api/info")]
        public async Task<IActionResult> Execute2([FromBody] string input)
        {
            if (!GetEtiInfoRequest.CanCreate(input, out var errors))
            {
                return BadRequest(_viewModel.Fail(errors.Select(err => $"- {err}").Aggregate((x, y) => $"{x}\n{y}")));
            }

            var request = GetEtiInfoRequest.Create(input);
            try
            {
                _ = await _mediator.Send(request).ConfigureAwait(false);
                return _viewModel.IsSuccess ? Ok(_viewModel) : StatusCode(500, _viewModel);
            }
            catch (Exception ex)
            {
                var innermostException = ex;
                while (innermostException.InnerException != null) innermostException = ex.InnerException!;
                return StatusCode(500, _viewModel.Fail(innermostException.Message));
            }
        }
    }
}