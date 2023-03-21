using GT.Trace.Changeover.App.UseCases.ApplyChangeover;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.Changeover.UI.HttpApi.EndPoints.Lines.ApplyChangeover
{
    public class ApplyChangeoverPresenter<T> : IPresenter<ApplyChangeoverSuccessResponse>, IPresenter<ApplyChangeoverFailureResponse>
        where T : ApplyChangeoverResponse
    {
        private readonly GenericViewModel<ApplyChangeoverEndPoint> _model;

        public ApplyChangeoverPresenter(GenericViewModel<ApplyChangeoverEndPoint> model)
        {
            _model = model;
        }

        public Task Handle(ApplyChangeoverSuccessResponse notification, CancellationToken cancellationToken)
        {
            _model.OK(notification.PrintExceptions);
            return Task.CompletedTask;
        }

        public Task Handle(ApplyChangeoverFailureResponse notification, CancellationToken cancellationToken)
        {
            _model.Fail(notification.Message);
            return Task.CompletedTask;
        }
    }
}