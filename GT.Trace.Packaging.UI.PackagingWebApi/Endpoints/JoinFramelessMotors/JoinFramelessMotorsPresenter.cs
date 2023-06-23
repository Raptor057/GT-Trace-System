using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.UseCases.JoinFramelessMotors;
using GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.SetLineHeadcount;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.JoinFramelessMotors
{

    public sealed class JoinFramelessMotorsPresenter<T> : IPresenter<JoinFramelessMotorsSuccess>, IPresenter<JoinFramelessMotorsFailure>
        where T : JoinFramelessMotorsResponse
    {
        private readonly GenericViewModel<SetLineHeadcountController> _viewModel;

        public JoinFramelessMotorsPresenter(GenericViewModel<SetLineHeadcountController> viewModel)
        {
            _viewModel=viewModel;
        }

        #pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task Handle(JoinFramelessMotorsSuccess notification, CancellationToken cancellationToken)
        #pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            _viewModel.OK(notification.Message ?? "");
        }

        public Task Handle(JoinFramelessMotorsFailure notification, CancellationToken cancellationToken)
        {
            _viewModel.Fail(notification.Message ?? "");
            return Task.CompletedTask;
        }
    }
}
