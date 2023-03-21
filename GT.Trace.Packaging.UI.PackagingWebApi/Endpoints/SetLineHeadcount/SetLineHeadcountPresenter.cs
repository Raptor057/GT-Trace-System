using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.UseCases.SetLineHeadcount;

namespace GT.Trace.Packaging.UI.PackagingWebApi.Endpoints.SetLineHeadcount
{
    public sealed class SetLineHeadcountPresenter<T> : IPresenter<SetLineHeadcountResponse>
        where T : SetLineHeadcountResponse
    {
        private readonly GenericViewModel<SetLineHeadcountController> _viewModel;

        public SetLineHeadcountPresenter(GenericViewModel<SetLineHeadcountController> viewModel)
        {
            _viewModel = viewModel;
        }

        public Task Handle(SetLineHeadcountResponse notification, CancellationToken cancellationToken)
        {
            if (notification is SetLineHeadcountFailure failure)
            {
                _viewModel.Fail(failure.Message);
            }
            else
            {
                _viewModel.OK(true);
            }
            return Task.CompletedTask;
        }
    }
}