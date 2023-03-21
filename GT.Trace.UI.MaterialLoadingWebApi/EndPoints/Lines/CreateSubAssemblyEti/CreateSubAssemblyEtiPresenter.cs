using GT.Trace.App.UseCases.Lines.GetNewSubAssemblyID;
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.UI.MaterialLoadingWebApi.Hubs;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GT.Trace.UI.MaterialLoadingWebApi.EndPoints.Lines.CreateSubAssemblyEti
{
    public sealed class CreateSubAssemblyEtiPresenter<T> : IResultPresenter<GetNewSubAssemblyIDResponse>
        where T : Result<GetNewSubAssemblyIDResponse>
    {
        private readonly ResultViewModel<CreateSubAssemblyEtiController> _viewModel;

        private readonly IHubContext<PointsOfUseHub, IPointsOfUseHub> _hub;

        public CreateSubAssemblyEtiPresenter(ResultViewModel<CreateSubAssemblyEtiController> viewModel, IHubContext<PointsOfUseHub, IPointsOfUseHub> hub)
        {
            _viewModel = viewModel;
            _hub = hub;
        }

        public async Task Handle(Result<GetNewSubAssemblyIDResponse> notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Set(failure);
            }
            else if (notification is ISuccess<GetNewSubAssemblyIDResponse> success)
            {
                _viewModel.Set(success);
                var response = success.Data;
                await _hub.Clients.All.EtiCreated(response.LineCode, response.EtiID, response.ComponentNo, response.Revision, response.CompDescription, response.Quantity, DateTime.UtcNow);
                await _hub.Clients.All.EtiCreated(response.TargetLineCode, response.EtiID, response.ComponentNo, response.Revision, response.CompDescription, response.Quantity, DateTime.UtcNow);
            }
        }
    }
}