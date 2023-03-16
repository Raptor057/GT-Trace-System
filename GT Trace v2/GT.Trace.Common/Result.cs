namespace GT.Trace.Common
{
    public abstract class Result : MediatR.INotification
    {
        public static Result<T> OK<T>(T data) => new SuccessResult<T>(data);

        public static Result<T> Fail<T>(string message) => new FailureResult<T>(message);

        //public static Result Fail(string message) => new FailureResult(message);

        public static Result<T> Fail<T>(Exception ex) => new FailureResult<T>(ex);

        //public static Result Fail(Exception ex) => new FailureResult(ex);
    }

    public abstract class Result<T> : MediatR.INotification
    {
        //public static implicit operator Result<T>(Result result)
        //{
        //    if (result is IFailure failure) return Result.Fail<T>(failure.Message);
        //    else if (result is ISuccess success) return Result.OK<T>(default);
        //    throw new InvalidOperationException("Tipo incorrecto.");
        //}
    }
}