namespace GT.Trace.Packaging.App.UseCases.PackUnit.Responses
{
    public sealed record UnitNotFoundResponse(long UnitID) : FailurePackUnitResponse($"Pieza #{UnitID} no encontrada.");
}