namespace GT.Trace.Common
{
    //public sealed class FailureResult : Result, IFailure
    //{
    //    public FailureResult(string message) => Exception = new Exception(message);

    //    public FailureResult(Exception ex) => Exception = ex;

    //    public Exception Exception { get; }

    //    public string Message => Exception.Message;

    //    public static implicit operator FailureResult(Exception ex) => new(ex);
    //}

    public sealed class FailureResult<T> : Result<T>, IFailure
    {
        public FailureResult(string message) => Exception = new Exception(message);

        public FailureResult(Exception ex) => Exception = ex;

        public Exception Exception { get; }

        public string Message => Exception.Message;

        //public static implicit operator FailureResult<T>(Exception ex) => new(ex);

        //public static implicit operator FailureResult<T>(FailureResult source) => new(source.Exception);
    }
}