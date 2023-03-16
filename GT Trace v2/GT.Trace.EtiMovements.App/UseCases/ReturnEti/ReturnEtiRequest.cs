using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.EtiMovements.App.UseCases.ReturnEti
{
    public sealed class ReturnEtiRequest : IResultRequest<ReturnEtiResponse>
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

        public static ReturnEtiRequest Create(string? lineCode, string? etiInput, bool isChangeOver)
        {
            if (!CanCreate(lineCode, etiInput, out var errors)) throw errors.AsException();
            return new(lineCode!, etiInput!, isChangeOver);
        }

        public ReturnEtiRequest(string lineCode, string etiInput, bool isChangeOver)
        {
            LineCode = lineCode;
            EtiInput = etiInput;
            IsChangeOver = isChangeOver;
        }

        public bool IsChangeOver { get; }

        public string LineCode { get; }

        public string EtiInput { get; }
    }
}