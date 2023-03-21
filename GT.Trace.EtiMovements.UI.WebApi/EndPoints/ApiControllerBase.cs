using GT.Trace.Common.CleanArch;
using Microsoft.AspNetCore.Mvc;

namespace GT.Trace.EtiMovements.UI.WebApi.EndPoints
{
    [ApiController]
    public class ApiControllerBase<T> : ControllerBase
    {
        protected ApiControllerBase(MediatR.IMediator mediator, ResultViewModel<T> viewModel, ILogger<T> logger)
        {
            Mediator = mediator;
            ViewModel = viewModel;
            Logger = logger;
        }

        protected ILogger<T> Logger { get; }

        protected ResultViewModel<T> ViewModel { get; }

        protected MediatR.IMediator Mediator { get; }

        public async Task<IActionResult> ExecuteAsync<TResponse>(IResultRequest<TResponse> request)
        {
            try
            {
                _ = await Mediator.Send(request).ConfigureAwait(false);
                return ViewModel.IsSuccess ? Ok(ViewModel) : StatusCode(500, ViewModel);
            }
            catch (Exception ex)
            {
                var innermostException = ex;
                while (innermostException.InnerException != null) innermostException = innermostException.InnerException;
                return StatusCode(500, new { innermostException.Message, UtcTimeStamp = DateTime.UtcNow, IsSuccess = false });
            }
        }
    }
}