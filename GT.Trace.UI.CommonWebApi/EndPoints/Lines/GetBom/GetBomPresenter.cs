using GT.Trace.App.UseCases.Lines.GetBom;
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.UI.CommonWebApi.EndPoints.Lines.GetBom
{
    public sealed class GetBomPresenter<T> : IResultPresenter<GetBomResponse>
        where T : Result<GetBomResponse>
    {
        private readonly ResultViewModel<GetBomController> _viewModel;

        public GetBomPresenter(ResultViewModel<GetBomController> viewModel)
        {
            _viewModel = viewModel;
        }

        public Task Handle(Result<GetBomResponse> notification, CancellationToken cancellationToken)
        {
            if (notification is IFailure failure)
            {
                _viewModel.Set(failure);
            }
            else if (notification is ISuccess<GetBomResponse> success)
            {
                _viewModel.Set(success, (data) => data.Bom);
            }
            return Task.CompletedTask;
        }
    }
}