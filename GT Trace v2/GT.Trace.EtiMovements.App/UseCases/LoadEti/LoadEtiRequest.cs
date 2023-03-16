using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.EtiMovements.App.UseCases.LoadEti
{
    public sealed class LoadEtiRequest : IResultRequest<LoadEtiResponse>
    {
        public static bool CanCreate(string? lineCode, string? pointOfUseCode, string? etiInput, out ErrorList errors)
        {
            errors = new();
            if (string.IsNullOrWhiteSpace(lineCode))
            {
                errors.Add("El parámetro de línea es requerido y se encuentra en blanco.");
            }
            if (string.IsNullOrWhiteSpace(pointOfUseCode))
            {
                errors.Add("El parámetro de túnel es requerido y se encuentra en blanco.");
            }
            if (string.IsNullOrWhiteSpace(etiInput))
            {
                errors.Add("El parámetro de ETI es requerido y se encuentra en blanco.");
            }
            return errors.IsEmpty;
        }

        public static LoadEtiRequest Create(string? lineCode, string? pointOfUseCode, string? etiInput, bool ignoreCapacity)
        {
            if (!CanCreate(lineCode, pointOfUseCode, etiInput, out var errors)) throw errors.AsException();
            return new(lineCode!, pointOfUseCode!, etiInput!, ignoreCapacity);
        }

        private LoadEtiRequest(string lineCode, string pointOfUseCode, string etiInput, bool ignoreCapacity)
        {
            LineCode = lineCode;
            PointOfUseCode = pointOfUseCode;
            EtiInput = etiInput;
            IgnoreCapacity = ignoreCapacity;
        }

        public string LineCode { get; }

        public string PointOfUseCode { get; }

        public string EtiInput { get; }

        public bool IgnoreCapacity { get; }
    }
}