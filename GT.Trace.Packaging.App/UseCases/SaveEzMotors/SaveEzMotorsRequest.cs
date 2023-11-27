using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.Packaging.App.UseCases.SaveEzMotors
{
    public sealed class SaveEzMotorsRequest:IRequest<SaveEzMotorsResponse>
    {
        public static bool CanCreate(string scannerInputEzQR, out ErrorList errors)
        {
            errors = new();
            if (string.IsNullOrEmpty(scannerInputEzQR))
            {
                errors.Add("La lectura del QR se encuentra en blanco y es requerida.");
            }
            return errors.IsEmpty;
        }

        public static SaveEzMotorsRequest Create(string modelo, string scannerInputEzQR, string lineCode)
        {
            if(!CanCreate(scannerInputEzQR,out var errors)) throw errors.AsException();
            return new(modelo, scannerInputEzQR, lineCode);
        }

        public SaveEzMotorsRequest(string model, string scannerInputEzQR, string lineCode)
        {
            Model = model;
            ScannerInputEzQR = scannerInputEzQR;
            LineCode = lineCode;
        }

        public string Model { get; }
        public string ScannerInputEzQR { get; }
        public string LineCode { get; }
    }
}
