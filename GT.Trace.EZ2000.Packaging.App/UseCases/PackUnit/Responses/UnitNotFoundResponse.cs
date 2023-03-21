namespace GT.Trace.EZ2000.Packaging.App.UseCases.PackUnit.Responses
{
    public sealed record UnitNotFoundResponse(long UnitID) : FailurePackUnitResponse($"Pieza #{UnitID} no encontrada.");
}