using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.EtiMovements.App.UseCases.UnloadEti;
using GT.Trace.EtiMovements.UI.WebApi.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace GT.Trace.EtiMovements.UI.WebApi.EndPoints.Lines.RemoveEti
{
    public sealed class UnloadEtiPresenter<T> : IResultPresenter<UnloadEtiResponse>
        where T : Result<UnloadEtiResponse>
    {
        private readonly ResultViewModel<RemoveEtiEndPoint> _viewModel;

        private readonly IHubContext<EtiMovementsHub, IEtiMovementsHub> _hub;

        public UnloadEtiPresenter(ResultViewModel<RemoveEtiEndPoint> viewModel, IHubContext<EtiMovementsHub, IEtiMovementsHub> hub)
        {
            _viewModel = viewModel;
            _hub = hub;
        }

        public Task Handle(Result<UnloadEtiResponse> notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Set(failure);
            }
            else if (notification is ISuccess<UnloadEtiResponse> success)
            {
                _viewModel.Set(success);
                _hub.Clients.All.EtiUnloaded(success.Data.LineCode, success.Data.EtiNo, success.Data.ComponentNo, success.Data.PointOfUseCode);
            }
            return Task.CompletedTask;
        }
    }
}