namespace GT.Trace.UI.CegidRadioWebApi.EndPoints
{
    public abstract record HttpApiResponse(bool IsSuccess)
    {
        public record SuccessResponse() : HttpApiResponse(true);

        public record SuccessResponse<TData>(TData Data) : SuccessResponse();

        public record FailureResponse(string Message) : HttpApiResponse(false);

        public static HttpApiResponse OK() => new SuccessResponse();

        public static HttpApiResponse OK<TData>(TData data) => new SuccessResponse<TData>(data);

        public static HttpApiResponse Fail(string message) => new FailureResponse(message);

        public DateTime UtcTimeStamp => DateTime.UtcNow;
    }
}