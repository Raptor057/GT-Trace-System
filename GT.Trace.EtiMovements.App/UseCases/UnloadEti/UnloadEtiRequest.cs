using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.EtiMovements.App.UseCases.UnloadEti
{
    public sealed class UnloadEtiRequest : IResultRequest<UnloadEtiResponse>
    {
        public static bool CanCreate(string? lineCode, string? etiInput, out ErrorList errors)
        {
            errors = new();
            if (string.IsNullOrWhiteSpace(lineCode))
            {
                errors.Add("El parámetro de línea es requerido y se encuentra en blanco.");
            }
            if (string.IsNullOrWhiteSpace(etiInput))
            {
                errors.Add("El parámetro de ETI es requerido y se encuentra en blanco.");
            }
            return errors.IsEmpty;
        }

        public static UnloadEtiRequest Create(string? lineCode, string? etiInput)
        {
            if (!CanCreate(lineCode, etiInput, out var errors)) throw errors.AsException();
            return new(lineCode!, etiInput!);
        }

        public UnloadEtiRequest(string lineCode, string etiInput)
        {
            LineCode = lineCode;
            EtiInput = etiInput;
        }

        public string LineCode { get; }

        public string EtiInput { get; }
    }
}