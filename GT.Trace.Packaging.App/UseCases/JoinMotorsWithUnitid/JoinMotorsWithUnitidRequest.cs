using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.UseCases.JoinMotorsWithUnitid.Responses;

namespace GT.Trace.Packaging.App.UseCases.JoinMotorsWithUnitid
{
    public sealed class JoinMotorsWithUnitidRequest:IRequest<JoinMotorsWithUnitidResponse>
    {
        public static bool CanCreate(string? scannerImput, string? lineCode, out ErrorList errors)
        {
            errors = new();
            if(string.IsNullOrEmpty(scannerImput))
            {
                errors.Add("La lectura se encuentra en blanco y es requerisa.");
            }
            if(string.IsNullOrEmpty(lineCode))
            {
                errors.Add("El codigo de la linea se encuentra en blanco y es requerido.");
            }
            return errors.IsEmpty;
        }
        public static JoinMotorsWithUnitidRequest Create(string? scannerImput, string? lineCode)
        {
            if(!CanCreate(scannerImput, lineCode, out var errors)) throw errors.AsException();
            return new(scannerImput!,lineCode!);
        }

        private JoinMotorsWithUnitidRequest(string scannerImput, string lineCode)
        {
            ScannerImput=scannerImput;
            LineCode = lineCode;
        }
        public string ScannerImput { get;}
        public string LineCode { get;}
    }
}
