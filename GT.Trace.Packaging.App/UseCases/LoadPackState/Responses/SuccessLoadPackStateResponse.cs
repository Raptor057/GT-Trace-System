namespace GT.Trace.Packaging.App.UseCases.LoadPackState.Responses
{
    public record SuccessLoadPackStateResponse(Dtos.PackStateDto State) : LoadPackStateResponse;
}