using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.EtiMovements.App.UseCases.GetEtiInfo;

namespace GT.Trace.EtiMovements.UI.WebApi.EndPoints.Etis.GetInfo
{
    public sealed class GetInfoPresenter<T> : IResultPresenter<GetEtiInfoResponse>
        where T : Result<GetEtiInfoResponse>
    {
        private readonly ResultViewModel<GetInfoEndPoint> _viewModel;

        public GetInfoPresenter(ResultViewModel<GetInfoEndPoint> viewModel)
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