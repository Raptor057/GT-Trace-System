using GT.Trace.App.UseCases.Lines.GetLine;
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.UI.MaterialLoadingWebApi.EndPoints.Lines.GetLine
{
    internal sealed class GetLinePresenter<T> : IResultPresenter<GetLineResponse>
        where T : Result<GetLineResponse>
    {
        private readonly ResultViewModel<GetLineController> _viewModel;

        public GetLinePresenter(ResultViewModel<GetLineController> viewModel)
        {
            _viewModel = viewModel;
        }

        public Task Handle(Result<GetLineResponse> notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Set(failure);
            }
            else if (notification is ISuccess<GetLineResponse> success)
            {
                _viewModel.Set(success, (data) => data.Line);
            }
            return Task.CompletedTask;
        }
    }
}