using GT.Trace.Packaging.App.Services;

namespace GT.Trace.Packaging.Infra.Services
{
    public class PrintingService : IPrintingService
    {
        public void PrintMasterLabel(long masterID) => Console.WriteLine($"Imprimir etiqueta master #{masterID}.");

        public void PrintJrLabel(int containerID) => Console.WriteLine($"Imprimir etiqueta Jr. #{containerID}");

        public void PrintPickingLabel(long unitID) => Console.WriteLine($"Imprimir etiqueta de picking para pieza #{unitID}");
    }
}