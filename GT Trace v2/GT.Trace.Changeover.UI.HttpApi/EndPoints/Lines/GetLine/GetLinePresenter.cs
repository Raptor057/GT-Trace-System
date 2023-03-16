using GT.Trace.Changeover.App.UseCases.GetLine;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.Changeover.UI.HttpApi.EndPoints.Lines.GetLine
{
    public class GetLinePresenter<T> : IPresenter<GetLineSuccessResponse>, IPresenter<GetLineFailureResponse>
        where T : GetLineResponse
    {
        private readonly GenericViewModel<GetLineEndPoint> _model;

        public GetLinePresenter(GenericViewModel<GetLineEndPoint> model)
        {
            _model = model;
        }

        public Task Handle(App.UseCases.GetLine.GetLineSuccessResponse notification, CancellationToken cancellationToken)
        {
            _model.OK(notification.WorkOrder);
            return Task.CompletedTask;
        }

        public Task Handle(GetLineFailureResponse notification, CancellationToken cancellationToken)
        {
            _model.Fail(notification.Message);
            return Task.CompletedTask;
        }
    }
}