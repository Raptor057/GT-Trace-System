using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.EtiMovements.App.UseCases.UseEti
{
    public sealed class UseEtiRequest : IResultRequest<UseEtiResponse>
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

        public static UseEtiRequest Create(string? lineCode, string? etiInput)
        {
            if (!CanCreate(lineCode, etiInput, out var errors)) throw errors.AsException();
            return new(lineCode!, etiInput!);
        }

        public UseEtiRequest(string lineCode, string etiInput)
        {
            LineCode = lineCode;
            EtiInput = etiInput;
        }

        public string LineCode { get; }

        public string EtiInput { get; }
    }
}