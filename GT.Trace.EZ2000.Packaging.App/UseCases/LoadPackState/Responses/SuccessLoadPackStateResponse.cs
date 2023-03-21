namespace GT.Trace.EZ2000.Packaging.App.UseCases.LoadPackState.Responses
{
    public record SuccessLoadPackStateResponse(Dtos.PackStateDto State) : LoadPackStateResponse;
}