namespace GT.Trace.App.UseCases.Lines.GetCurrentHourProduction
{
    public sealed record GetCurrentHourProductionRequest(string LineCode) : Common.CleanArch.IResultRequest<GetCurrentHourProductionResponse>;
}