using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.Packaging.App.UseCases.JoinEZMotors
{
    public sealed class JoinEZMotorsRequest:IRequest<JoinEZMotorsResponse>
    {


        public static bool CanCreate(string scannerInputUnitID, string scannerInputMotorID1, string scannerInputMotorID2, out ErrorList errors)
        {
            errors = new();
            if(string.IsNullOrEmpty(scannerInputUnitID))
            {
                errors.Add("La lectura de la etiqueta individual se encuentra en blanco y es requerida.");
            }
            if(string.IsNullOrEmpty(scannerInputMotorID1))
            {
                errors.Add("La lectura del QR del Motor 1 se encuentra en blanco y es requerida.");
            }
            if(string.IsNullOrEmpty(scannerInputMotorID2))
            {
                errors.Add("La lectura del QR del Motor 2 se encuentra en blanco y es requerida.");
            }
            return errors.IsEmpty;
        }

        public static JoinEZMotorsRequest Create(string scannerInputUnitID, string scannerInputMotorID1, string scannerInputMotorID2, int isEnable)
        {
            if (!CanCreate(scannerInputUnitID, scannerInputMotorID1, scannerInputMotorID2, out var errors)) throw errors.AsException();
            return new (scannerInputUnitID, scannerInputMotorID1!, scannerInputMotorID2!, isEnable!);
        }
        private JoinEZMotorsRequest(string scannerInputUnitID, string scannerInputMotorID1, string scannerInputMotorID2, int isEnable)
        {
            ScannerInputUnitID=scannerInputUnitID;
            ScannerInputMotorID1 = scannerInputMotorID1;
            ScannerInputMotorID2 = scannerInputMotorID2;
            IsEnable=isEnable;
            
        }
        public string ScannerInputUnitID { get; }
        public string ScannerInputMotorID1 { get; }
        public string ScannerInputMotorID2 { get; }
        public int IsEnable { get; }
    }
}
