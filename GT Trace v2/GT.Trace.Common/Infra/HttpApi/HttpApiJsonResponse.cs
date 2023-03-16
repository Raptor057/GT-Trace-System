namespace GT.Trace.Common.Infra.HttpApi
{
    public record HttpApiJsonResponse(bool IsSuccess, string? Message, DateTime UtcTimeStamp);

    public record HttpApiJsonResponse<TData>(bool IsSuccess, string? Message, TData? Data, DateTime UtcTimeStamp) : HttpApiJsonResponse(IsSuccess, Message, UtcTimeStamp);
}