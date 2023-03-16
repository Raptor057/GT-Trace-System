using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.EZ2000.Packaging.App.UseCases.Auth;
namespace GT.Trace.EZ2000.Packaging.UI.PackagingWebApi.Endpoints.Auth
{
    //public sealed class AuthPresenter<T>:IResultPresenter<AuthResponse> 
    //    where T : Result<AuthResponse>
    //{
    //    private readonly ResultViewModel<AuthController> _viewModel;
    //    public AuthPresenter(ResultViewModel<AuthController> viewModel)
    //    {
    //        _viewModel = viewModel;
    //    }
    //    public Task Handle(Result<AuthResponse> notification, CancellationToken cancellationToken)
    //    {
    //        if (notification is IFailure failure)
    //        {
    //            _viewModel.Set(failure);
    //        }
    //        else if (notification is ISuccess<AuthResponse> success)
    //        {
    //           _viewModel.Set(success);
    //        }
    //        return Task.CompletedTask;
    //    }
    //}
}
