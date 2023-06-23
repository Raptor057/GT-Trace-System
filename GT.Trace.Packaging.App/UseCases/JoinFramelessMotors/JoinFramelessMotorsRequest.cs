using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.Packaging.App.UseCases.JoinFramelessMotors
{
    public sealed class JoinFramelessMotorsRequest : IRequest<JoinFramelessMotorsResponse>
    {


        public static bool CanCreate(string scannerInputUnitID, string scannerInputComponentID, out ErrorList errors)
        {
            errors = new();
            if (string.IsNullOrEmpty(scannerInputUnitID))
            {
                errors.Add("La lectura de la etiqueta individual se encuentra en blanco y es requerida.");
            }
            if (string.IsNullOrEmpty(scannerInputComponentID))
            {
                errors.Add("La lectura del QR del Motor se encuentra en blanco y es requerida.");
            }
            return errors.IsEmpty;
        }

        public static JoinFramelessMotorsRequest Create(string scannerInputUnitID, string scannerInputComponentID, string lineCode, string partNo)
        {
            if(!CanCreate(scannerInputUnitID, scannerInputComponentID, out var errors)) throw errors.AsException();
            return new(scannerInputUnitID!, scannerInputComponentID!, lineCode, partNo);
        }
        private JoinFramelessMotorsRequest(string scannerInputUnitID, string scannerInputComponentID, string lineCode, string partNo)
        {
            ScannerInputUnitID = scannerInputUnitID;
            ScannerInputComponentID = scannerInputComponentID; 
            LineCode=lineCode;
            PartNo=partNo;
        }
        public string ScannerInputComponentID { get; }
        public string ScannerInputUnitID { get; }
        public string LineCode { get; }
        public string PartNo { get; }
    }
}
