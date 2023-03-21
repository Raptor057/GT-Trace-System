using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.Etis.App.UseCases.GetEtiInfo;

namespace GT.Trace.Etis.UI.WebApi.EndPoints
{
    public sealed class GetEtiInfoPresenter<T> : IResultPresenter<GetEtiInfoResponse>
        where T : Result<GetEtiInfoResponse>
    {
        private readonly ResultViewModel<GetEtiInfoController> _viewModel;

        public GetEtiInfoPresenter(ResultViewModel<GetEtiInfoController> viewModel)
        {
            _viewModel = viewModel;
        }

        public Task Handle(Result<GetEtiInfoResponse> notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Set(failure);
            }
            else if (notification is ISuccess<GetEtiInfoResponse> success)
            {
                _viewModel.Set(success);
            }
            return Task.CompletedTask;
        }
    }
}