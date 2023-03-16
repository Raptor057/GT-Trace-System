using GT.Trace.Common.CleanArch;

namespace GT.Trace.App.UseCases.Lines.GetLine
{
    public sealed class GetLineRequest : IResultRequest<GetLineResponse>
    {
        public static bool CanCreate(string lineCode, out List<string> errors)
        {
            errors = new();
            if (string.IsNullOrWhiteSpace(lineCode))
            {
                errors.Add("El código de línea es obligatorio pero se encuentra en blanco.");
            }
            return errors.Count == 0;
        }

        public static GetLineRequest Create(string lineCode)
        {
            if (!CanCreate(lineCode, out var errors)) throw new InvalidOperationException(errors.Select(e => $"- {e}").Aggregate((a, b) => $"{a}\n{b}"));
            return new(lineCode);
        }

        private GetLineRequest(string lineCode)
        {
            LineCode = lineCode;
        }

        public string LineCode { get; }
    }
}