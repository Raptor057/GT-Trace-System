using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.EtiMovements.App.UseCases.UseEti;
using GT.Trace.EtiMovements.UI.WebApi.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace GT.Trace.EtiMovements.UI.WebApi.EndPoints.Lines.UseEti
{
    public sealed class UseEtiPresenter<T> : IResultPresenter<UseEtiResponse>
        where T : Result<UseEtiResponse>
    {
        private readonly ResultViewModel<UseEtiEndPoint> _viewModel;

        private readonly IHubContext<EtiMovementsHub, IEtiMovementsHub> _hub;

        public UseEtiPresenter(ResultViewModel<UseEtiEndPoint> viewModel, IHubContext<EtiMovementsHub, IEtiMovementsHub> hub)
        {
            _viewModel = viewModel;
            _hub = hub;
        }

        public Task Handle(Result<UseEtiResponse> notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Set(failure);
            }
            else if (notification is ISuccess<UseEtiResponse> success)
            {
                _viewModel.Set(success);
                _hub.Clients.All.EtiUsed(success.Data.LineCode, success.Data.EtiNo, success.Data.ComponentNo, success.Data.PointOfUseCode);
            }
            return Task.CompletedTask;
        }
    }
}