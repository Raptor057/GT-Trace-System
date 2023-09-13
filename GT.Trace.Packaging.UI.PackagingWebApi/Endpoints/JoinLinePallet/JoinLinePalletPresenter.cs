using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.UseCases.JoinLinePallet;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.JoinPalletQR
{
    public sealed class JoinLinePalletPresenter<T> : IPresenter<JoinLinePalletSucess>, IPresenter<JoinLinePalletFailure> where T : JoinLinePalletResponse
    {
        private readonly GenericViewModel<JoinLinePalletController> _viewmodel;

        public JoinLinePalletPresenter(GenericViewModel<JoinLinePalletController> viewModel)
        {
            _viewmodel=viewModel;
        }
        public async Task Handle(JoinLinePalletSucess notification, CancellationToken cancellationToken)
        {
            _viewmodel.OK(notification.Message ?? "");
            await Task.CompletedTask;
        }
        public async Task Handle(JoinLinePalletFailure notification, CancellationToken cancellationToken)
        {
            _viewmodel.Fail(notification.Message ?? "");
            await Task.CompletedTask;
        }
    }
}
