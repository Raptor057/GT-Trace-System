using GT.Trace.Common.CleanArch;
using GT.Trace.EZ2000.Packaging.App.UseCases.UpdateActiveEti;

namespace GT.Trace.EZ2000.Packaging.UI.PackagingWebApi.Endpoints.UpdateActiveEti
{
    public sealed class UpdateEtiTrazaPresenter<T> : IPresenter<UpdateEtiTrazaResponse>
        where T : UpdateEtiTrazaResponse
    {
        private readonly GenericViewModel<UpdateActiveEtiController> _viewModel;

        public UpdateEtiTrazaPresenter(GenericViewModel<UpdateActiveEtiController> viewModel)
        {
            _viewModel=viewModel;
        }
        public Task Handle(UpdateEtiTrazaResponse notification, CancellationToken cancellationToken)
        {
            if(notification is UpdateEtiTrazaFailure failure)
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
