using GT.Trace.Changeover.App.UseCases.GetGamma;
using GT.Trace.Changeover.App.UseCases.GetLine;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.Changeover.UI.HttpApi.EndPoints.Lines.GetGamma
{
    public class GetGammaPresenter<T> : IPresenter<GetGammaSuccessResponse>, IPresenter<GetGammaFailureResponse>
        where T : GetGammaResponse
    {
        private readonly GenericViewModel<GetGammaEndPoint> _model;

        public GetGammaPresenter(GenericViewModel<GetGammaEndPoint> model)
        {
            _model = model;
        }

        public Task Handle(GetGammaSuccessResponse notification, CancellationToken cancellationToken)
        {
            _model.OK(notification.Gamma);
            return Task.CompletedTask;
        }

        public Task Handle(GetGammaFailureResponse notification, CancellationToken cancellationToken)
        {
            _model.Fail(notification.Message);
            return Task.CompletedTask;
        }
    }
}