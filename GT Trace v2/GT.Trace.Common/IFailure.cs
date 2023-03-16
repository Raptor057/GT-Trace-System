namespace GT.Trace.Common
{
    public interface IFailure
    {
        //Exception Exception { get; }

        string Message { get; }
    }

    //public abstract record XResponse(bool IsSuccess)
    //   {
    //       public record SuccessResponse() : XResponse(true);

    //       public record SuccessResponse<TData>(TData Data) : SuccessResponse();

    //       public record FailureResponse(string Message) : XResponse(false);

    //       public static XResponse OK() => new SuccessResponse();

    //       public static XResponse OK<TData>(TData data) => new SuccessResponse<TData>(data);

    //       public static XResponse Fail(string message) => new FailureResponse(message);

    //       public DateTime UtcTimeStamp => DateTime.UtcNow;
    //   }
}