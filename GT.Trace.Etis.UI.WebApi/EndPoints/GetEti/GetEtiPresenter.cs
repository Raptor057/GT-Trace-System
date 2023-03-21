using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.Etis.App.UseCases.GetEti;

namespace GT.Trace.Etis.UI.WebApi.EndPoints.GetEti
{
    public sealed class GetEtiPresenter<T> : IResultPresenter<GetEtiResponse>
        where T : Result<GetEtiResponse>
    {
        private readonly ResultViewModel<GetEtiEndPoint> _viewModel;

        public GetEtiPresenter(ResultViewModel<GetEtiEndPoint> viewModel)
        {
            _viewModel = viewModel;
        }

        public Task Handle(Result<GetEtiResponse> notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Set(failure);
            }
            else if (notification is ISuccess<GetEtiResponse> success)
            {
                _viewModel.Set(success);
            }
            return Task.CompletedTask;
        }
    }
}