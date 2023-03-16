using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.EtiMovements.App.UseCases.LoadEti;
using GT.Trace.EtiMovements.UI.WebApi.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace GT.Trace.EtiMovements.UI.WebApi.EndPoints.Lines.LoadEti
{
    public sealed class LoadEtiPresenter<T> : IResultPresenter<LoadEtiResponse>
        where T : Result<LoadEtiResponse>
    {
        private readonly ResultViewModel<LoadEtiEndPoint> _viewModel;

        private readonly IHubContext<EtiMovementsHub, IEtiMovementsHub> _hub;

        public LoadEtiPresenter(ResultViewModel<LoadEtiEndPoint> viewModel, IHubContext<EtiMovementsHub, IEtiMovementsHub> hub)
        {
            _viewModel = viewModel;
            _hub = hub;
        }

        public Task Handle(Result<LoadEtiResponse> notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Set(failure);
            }
            else if (notification is ISuccess<LoadEtiResponse> success)
            {
                _viewModel.Set(success);
                _hub.Clients.All.EtiLoaded(success.Data.LineCode, success.Data.EtiNo, success.Data.ComponentNo, success.Data.PointOfUseCode);
            }
            return Task.CompletedTask;
        }
    }
}