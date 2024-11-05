using GT.Trace.App.UseCases.Lines.UpdateBomLine;
using GT.Trace.App.UseCases.Lines.UpdateGama;
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.UI.CommonWebApi.EndPoints.Lines.UpdateBomLine
{
    public class UpdateBomLinePresenter<T> : IPresenter<UpdateBomLineResponse> where T : UpdateBomLineResponse
    {
        private readonly GenericViewModel<UpdateBomLineEndPoint> _viewModel;

        public UpdateBomLinePresenter(GenericViewModel<UpdateBomLineEndPoint> viewModel)
        {
            _viewModel = viewModel;
        }

        public async Task Handle(UpdateBomLineResponse notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Fail(failure.Message);
                await Task.CompletedTask;
            }
            else if (notification is UpdateBomLineSuccessResponse success)
            {
                _viewModel.OK(success);
                await Task.CompletedTask;
            }
        }
    }
}
