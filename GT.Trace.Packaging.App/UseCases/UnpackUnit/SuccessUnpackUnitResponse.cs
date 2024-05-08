namespace GT.Trace.Packaging.App.UseCases.UnpackUnit
{
    public record SuccessUnpackUnitResponse(string LineCode, long UnitID, string LineName, string WorkOrderCode) : UnpackUnitResponse;
}