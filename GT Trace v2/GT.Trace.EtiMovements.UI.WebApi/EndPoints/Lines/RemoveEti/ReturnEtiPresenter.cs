using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.EtiMovements.App.UseCases.ReturnEti;
using GT.Trace.EtiMovements.UI.WebApi.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace GT.Trace.EtiMovements.UI.WebApi.EndPoints.Lines.RemoveEti
{
    public sealed class ReturnEtiPresenter<T> : IResultPresenter<ReturnEtiResponse>
        where T : Result<ReturnEtiResponse>
    {
        private readonly ResultViewModel<RemoveEtiEndPoint> _viewModel;

        private readonly IHubContext<EtiMovementsHub, IEtiMovementsHub> _hub;

        public ReturnEtiPresenter(ResultViewModel<RemoveEtiEndPoint> viewModel, IHubContext<EtiMovementsHub, IEtiMovementsHub> hub)
        {
            _viewModel = viewModel;
            _hub = hub;
        }

        public Task Handle(Result<ReturnEtiResponse> notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Set(failure);
            }
            else if (notification is ISuccess<ReturnEtiResponse> success)
            {
                _viewModel.Set(success);
                _hub.Clients.All.EtiReturned(success.Data.LineCode, success.Data.EtiNo, success.Data.PartNo, success.Data.ComponentNo, success.Data.PointOfUseCode, success.Data.OperatorNo, success.Data.UtcTimeStamp);
            }
            return Task.CompletedTask;
        }
    }
}