using GT.Trace.Changeover.App.UseCases.GetWorkOrder;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.Changeover.UI.HttpApi.EndPoints.WorkOrders.GetWorkOrderByLineID
{
    public class GetWorkOrderByLineIDPresenter<T> : IPresenter<GetWorkOrderSuccessResponse>, IPresenter<GetWorkOrderFailureResponse>
        where T : GetWorkOrderResponse
    {
        private readonly GenericViewModel<GetWorkOrderByLineIDEndPoint> _model;

        public GetWorkOrderByLineIDPresenter(GenericViewModel<GetWorkOrderByLineIDEndPoint> model)
        {
            _model = model;
        }

        public Task Handle(GetWorkOrderSuccessResponse notification, CancellationToken cancellationToken)
        {
            _model.OK(notification.WorkOrder);
            return Task.CompletedTask;
        }

        public Task Handle(GetWorkOrderFailureResponse notification, CancellationToken cancellationToken)
        {
            _model.Fail(notification.Message);
            return Task.CompletedTask;
        }
    }
}