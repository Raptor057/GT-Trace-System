using GT.Trace.Etis.App.UseCases.ParseEti;
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.Etis.UI.WebApi.EndPoints.ParseEti
{
    public sealed class ParseEtiPresenter<T> : IResultPresenter<ParseEtiResponse>
        where T : Result<ParseEtiResponse>
    {
        private readonly ResultViewModel<ParseEtiEndPoint> _viewModel;

        public ParseEtiPresenter(ResultViewModel<ParseEtiEndPoint> viewModel)
        {
            _viewModel = viewModel;
        }

        public Task Handle(Result<ParseEtiResponse> notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Set(failure);
            }
            else if (notification is ISuccess<ParseEtiResponse> success)
            {
                _viewModel.Set(success);
            }
            return Task.CompletedTask;
        }
    }
}