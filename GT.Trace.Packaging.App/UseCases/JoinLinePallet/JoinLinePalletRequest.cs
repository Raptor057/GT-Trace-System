using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.Packaging.App.UseCases.JoinLinePallet
{
    public sealed class JoinLinePalletRequest:IRequest<JoinLinePalletResponse>
    {
        public static bool CanCreate(string scannerInputUnit, string scannerInputPallet, out ErrorList errors)
        {
            errors = new();
            if(string.IsNullOrEmpty(scannerInputUnit))
            {
                errors.Add("La lectura de la etiqueta individual se encuentra en blanco y es requerida.");
            }
            if(string.IsNullOrEmpty(scannerInputPallet))
            {
                errors.Add("La lectura del pallet se encuentra en blanco y es requerida.");
            }
            return errors.IsEmpty;
        }
        public static JoinLinePalletRequest Create(string scannerInputUnit, string scannerInputPallet, string lineCode,int isEnable)
        {
            if (!CanCreate(scannerInputUnit, scannerInputPallet, out var errors)) throw errors.AsException();
            return new JoinLinePalletRequest(scannerInputUnit!, scannerInputPallet!,lineCode, isEnable!);
        }
        private JoinLinePalletRequest(string scannerInputUnit, string scannerInputPallet, string lineCode,int isEnable)
        {
            ScannerInputUnitID = scannerInputUnit;
            ScannerInputPallet = scannerInputPallet;
            LineCode=lineCode;
            IsEnable =isEnable;
        }
        public string ScannerInputUnitID { get; }
        public string ScannerInputPallet { get;}
        public string LineCode { get; }
        public int IsEnable { get;}
    }
}
