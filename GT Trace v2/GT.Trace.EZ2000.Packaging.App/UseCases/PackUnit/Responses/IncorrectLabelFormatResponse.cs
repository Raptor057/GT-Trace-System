namespace GT.Trace.EZ2000.Packaging.App.UseCases.PackUnit.Responses
{
    public sealed record IncorrectLabelFormatResponse(string? Input) : FailurePackUnitResponse($"Formato de etiqueta equivocado \"{Input}\".");
}