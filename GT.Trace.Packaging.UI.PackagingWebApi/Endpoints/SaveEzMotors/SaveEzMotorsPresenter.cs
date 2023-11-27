using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.UseCases.SaveEzMotors;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.SaveEzMotors
{
    public class SaveEzMotorsPresenter<T> : IPresenter<SaveEzMotorsSuccess>, IPresenter<SaveEzMotorsFailure> where T : SaveEzMotorsResponse
    {
        private readonly GenericViewModel<SaveEzMotorsController> _viewmodel;

        public SaveEzMotorsPresenter(GenericViewModel<SaveEzMotorsController> viewmodel)
        {
            _viewmodel=viewmodel;
        }
        public async Task Handle(SaveEzMotorsSuccess notification, CancellationToken cancellationToken)
        {
            _viewmodel.OK(notification.Message ?? "");
            await Task.CompletedTask;
        }

        public Task Handle(SaveEzMotorsFailure notification, CancellationToken cancellationToken)
        {
            _viewmodel.Fail(notification.Message ?? "");
            return Task.CompletedTask;
        }
    }
}
