namespace GT.Trace.App.UseCases.Lines.GetHourlyProduction
{
    public sealed record GetHourlyProductionRequest(string LineCode) : Common.CleanArch.IResultRequest<GetHourlyProductionResponse>;
}