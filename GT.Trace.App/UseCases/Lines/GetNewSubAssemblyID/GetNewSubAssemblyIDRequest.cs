using GT.Trace.Common.CleanArch;

namespace GT.Trace.App.UseCases.Lines.GetNewSubAssemblyID
{
    public sealed class GetNewSubAssemblyIDRequest : IResultRequest<GetNewSubAssemblyIDResponse>
    {
        public static bool CanCreate(string? lineCode, out List<string> errors)
        {
            errors = new();
            if (string.IsNullOrWhiteSpace(lineCode))
            {
                errors.Add("El código de línea se encuentra en blanco.");
            }
            return errors.Count == 0;
        }

        public static GetNewSubAssemblyIDRequest Create(string? lineCode)
        {
            if (!CanCreate(lineCode, out var errors)) throw new InvalidOperationException(errors.Select(e => $"- {e}").Aggregate((a, b) => $"{a}\n{b}"));
            return new(lineCode!);
        }

        private GetNewSubAssemblyIDRequest(string lineCode)
        {
            LineCode = lineCode;
        }

        public string LineCode { get; }
    }
}