using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.EZ2000.Packaging.App.UseCases.PackUnit.Responses;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.PackUnit
{
    public sealed class PackUnitRequest : IRequest<PackUnitResponse>
    {
        public static bool CanCreate(string? scannerInput, string? hostname, out ErrorList errors)
        {
            errors = new();
            if (string.IsNullOrEmpty(scannerInput))
            {
                errors.Add("La lectura se encuentra en blanco y es requerida.");
            }
            if (string.IsNullOrEmpty(hostname))
            {
                errors.Add("El nombre del equipo se encuentra en blanco y es requerido.");
            }
            return errors.IsEmpty;
        }

        public static PackUnitRequest Create(string? scannerInput, string? hostname, int? palletSize, int? containerSize, string? lineCode, string? poNumber)
        {
            if (!CanCreate(scannerInput, hostname, out var errors)) throw errors.AsException();
            return new(scannerInput!, hostname!, palletSize, containerSize, lineCode, poNumber);
        }

        private PackUnitRequest(string scannerInput, string hostname, int? palletSize, int? containerSize, string? lineCode, string? poNumber)
        {
            ScannerInput = scannerInput;
            Hostname = hostname;
            PalletSize = palletSize;
            ContainerSize = containerSize;
            LineCode = lineCode;
            PoNumber = poNumber;
        }

        public string? PoNumber { get; }

        public string ScannerInput { get; }

        public string Hostname { get; }

        public int? PalletSize { get; }

        public int? ContainerSize { get; }

        public string? LineCode { get; }
    }
}