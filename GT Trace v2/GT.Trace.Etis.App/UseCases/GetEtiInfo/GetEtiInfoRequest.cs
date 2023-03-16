using GT.Trace.Common.CleanArch;

namespace GT.Trace.Etis.App.UseCases.GetEtiInfo
{
    public sealed class GetEtiInfoRequest : IResultRequest<GetEtiInfoResponse>
    {
        public static bool CanCreate(string? scannerInput, out List<string> errors)
        {
            errors = new();
            if (string.IsNullOrWhiteSpace(scannerInput))
            {
                errors.Add("La captura del escaner se pasó en blanco.");
            }
            return errors.Count == 0;
        }

        public static GetEtiInfoRequest Create(string? scannerInput)
        {
            if (!CanCreate(scannerInput, out var errors)) throw new InvalidOperationException(errors.Select(err => $"- {err}").Aggregate((x, y) => $"{x}\n{y}"));
            return new(scannerInput!);
        }

        private GetEtiInfoRequest(string scannerInput)
        {
            ScannerInput = scannerInput;
        }

        public string ScannerInput { get; }
    }
}