namespace GT.Trace.EZ2000.Packaging.App.UseCases.UnpackUnit
{
    public record SuccessUnpackUnitResponse(string LineCode, long UnitID) : UnpackUnitResponse;
}