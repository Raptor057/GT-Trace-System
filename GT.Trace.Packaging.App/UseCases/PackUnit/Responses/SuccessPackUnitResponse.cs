namespace GT.Trace.Packaging.App.UseCases.PackUnit.Responses
{
    public abstract record SuccessPackUnitResponse(string LineCode, long UnitID) : PackUnitResponse;
}