//using GT.Trace.Common;
//using GT.Trace.Common.CleanArch;
//using GT.Trace.EtiMovements.App.UseCases.UpdateEtiTraza;
//using GT.Trace.EtiMovements.App.UseCases.UseEti;
//using GT.Trace.EtiMovements.UI.WebApi.Hubs;
//using Microsoft.AspNetCore.SignalR;

//namespace GT.Trace.EtiMovements.UI.WebApi.EndPoints.Lines.UpdateEtiTraza
//{
//    public sealed class UpdateEtiTrazaPresenter<T> : IResultPresenter<UpdateEtiTrazaResponse>
//        where T : Result<UpdateEtiTrazaResponse>
//    {
//        private readonly ResultViewModel<UpdateEtiTrazaEndPoint> _viewModel;
//        private readonly IHubContext<EtiMovementsHub, IEtiMovementsHub> _hub;

//        public UpdateEtiTrazaPresenter(ResultViewModel<UpdateEtiTrazaEndPoint> viewModel,IHubContext<EtiMovementsHub, IEtiMovementsHub> hub)
//        {
//            _viewModel = viewModel;
//            _hub = hub;
//        }

//        public Task Handle(Result<UpdateEtiTrazaResponse> notification, CancellationToken cancellationToken)
//        {
//            if (notification is IFailure failure)
//            {
//                _viewModel.Fail(failure.Message);
//            }
//            else if (notification is ISuccess<UseEtiResponse> success)
//            {
                
//                _viewModel.Set(success);
//                _hub.Clients.All.UpdateEtiTraza(success.Data.EtiNo);
//            }
//            return Task.CompletedTask;
//        }
//    }
//}