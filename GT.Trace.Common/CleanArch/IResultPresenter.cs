namespace GT.Trace.Common.CleanArch
{
    public interface IResultPresenter<TResponse> : MediatR.INotificationHandler<Result<TResponse>>
    { }
}