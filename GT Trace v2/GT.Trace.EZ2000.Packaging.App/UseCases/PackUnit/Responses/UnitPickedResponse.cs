namespace GT.Trace.EZ2000.Packaging.App.UseCases.PackUnit.Responses
{
    public sealed record UnitPickedResponse(string LineCode, long UnitID)
        : SuccessPackUnitResponse(LineCode, UnitID);
}