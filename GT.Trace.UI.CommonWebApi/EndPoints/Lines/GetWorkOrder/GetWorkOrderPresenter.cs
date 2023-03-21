using GT.Trace.App.UseCases.Lines.GetWorkOrder;
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.UI.CommonWebApi.EndPoints.Lines.GetWorkOrder
{
    public sealed class GetWorkOrderPresenter<T> : IResultPresenter<GetWorkOrderResponse>
        where T : Result<GetWorkOrderResponse>
    {
        private readonly ResultViewModel<GetWorkOrderController> _viewModel;

        public GetWorkOrderPresenter(ResultViewModel<GetWorkOrderController> viewModel)
        {
            _viewModel = viewModel;
        }

        public Task Handle(Result<GetWorkOrderResponse> notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Set(failure);
            }
            else if (notification is ISuccess<GetWorkOrderResponse> success)
            {
                _viewModel.Set(success, data => data.WorkOrder);
            }
            return Task.CompletedTask;
        }
    }
}