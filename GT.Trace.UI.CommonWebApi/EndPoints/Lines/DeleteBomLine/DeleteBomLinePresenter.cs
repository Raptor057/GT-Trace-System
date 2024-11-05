using GT.Trace.App.UseCases.Lines.DeleteBomLine;
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.UI.CommonWebApi.EndPoints.Lines.DeleteBomLine
{
    public class DeleteBomLinePresenter<T> : IPresenter<DeleteBomLineResponse> where T : DeleteBomLineResponse
    {
        private readonly GenericViewModel<DeleteBomLineEndPoint> _viewModel;

        public DeleteBomLinePresenter(GenericViewModel<DeleteBomLineEndPoint> viewModel)
        {
            _viewModel = viewModel;
        }
        public async Task Handle(DeleteBomLineResponse notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Fail(failure.Message);
                await Task.CompletedTask;
            }
            else if (notification is DeleteBomLineSuccessResponse success)
            {
                _viewModel.OK(success);
                await Task.CompletedTask;
            }
        }
    }
}
