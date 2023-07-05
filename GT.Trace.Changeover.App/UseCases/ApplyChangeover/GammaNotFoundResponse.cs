namespace GT.Trace.Changeover.App.UseCases.ApplyChangeover
{
    public sealed record GammaNotFoundResponse(string Message)
        :ApplyChangeoverFailureResponse($"{Message}.");
}
