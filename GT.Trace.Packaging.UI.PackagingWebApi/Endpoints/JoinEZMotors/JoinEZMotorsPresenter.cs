using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.UseCases.JoinEZMotors;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.JoinEZMotors
{
    public sealed class JoinEZMotorsPresenter<T> : IPresenter<JoinEZMotorsSuccess>, IPresenter<JoinEZMotorsFailure> where T : JoinEZMotorsResponse
    {
        private readonly GenericViewModel<JoinEZMotorsController> _viewmodel;

        public JoinEZMotorsPresenter(GenericViewModel<JoinEZMotorsController> viewmodel)
        {
            _viewmodel=viewmodel;
        }

        public async Task Handle(JoinEZMotorsSuccess notification, CancellationToken cancellationToken)
        {
            _viewmodel.OK(notification.Message ?? "");
            await Task.CompletedTask;
        }

        public Task Handle(JoinEZMotorsFailure notification, CancellationToken cancellationToken)
        {
            _viewmodel.Fail(notification.Message ?? "");
            return Task.CompletedTask;
        }
    }
}
