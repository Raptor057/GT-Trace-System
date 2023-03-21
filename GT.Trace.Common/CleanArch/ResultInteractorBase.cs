namespace GT.Trace.Common.CleanArch
{
    public abstract class ResultInteractorBase<TRequest, TResult> : IResultInteractor<TRequest, TResult>
        where TRequest : IResultRequest<TResult>
    {
        protected Result<TResult> OK(TResult data) => Result.OK(data);

        protected Result<TResult> Fail(string message) => Result.Fail<TResult>(message);

        protected Result<TResult> Fail(Exception ex) => Result.Fail<TResult>(ex);

        public abstract Task<Result<TResult>> Handle(TRequest request, CancellationToken cancellationToken);
    }
}