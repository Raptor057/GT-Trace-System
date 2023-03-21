namespace GT.Trace.Packaging.App.UseCases.PackUnit.Responses
{
    public sealed record IncorrectLabelFormatResponse(string? Input) : FailurePackUnitResponse($"Formato de etiqueta equivocado \"{Input}\".");
}