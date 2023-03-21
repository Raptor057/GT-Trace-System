namespace GT.Trace.EZ2000.Packaging.App.UseCases.LoadPackState.Responses
{
    public sealed record WrongLinePartNumberResponse(string WorkOrderCode, string LinePartNo, string WorkOrderPartNo)
        : FailureLoadPackStateResponse($"El modelo cargado en la línea no corresponde con el de la orden de fabricación [{WorkOrderCode}].\nSe requiere cambio de modelo: {LinePartNo} -> {WorkOrderPartNo}.");
}