using GT.Trace.App.UseCases.Lines.UpdateBomLine;
using GT.Trace.App.UseCases.Lines.UpdateGama;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.UI.CommonWebApi.EndPoints.Lines.UpdateBomLine
{
    public class UpdateBomLinePresenter<T> : IPresenter<UpdateBomLineSuccessResponse> where T : UpdateBomLineResponse
    {
        private readonly GenericViewModel<UpdateBomLineEndPoint> _model;

        public UpdateBomLinePresenter(GenericViewModel<UpdateBomLineEndPoint> model)
        {
            _model=model;
        }
        public Task Handle(UpdateBomLineSuccessResponse notification, CancellationToken cancellationToken)
        {
            _model.OK(notification);
            return Task.CompletedTask;
        }
    }
}
