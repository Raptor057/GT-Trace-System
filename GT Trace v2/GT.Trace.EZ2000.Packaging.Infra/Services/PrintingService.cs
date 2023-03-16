using GT.Trace.EZ2000.Packaging.App.Services;

namespace GT.Trace.EZ2000.Packaging.Infra.Services
{
    public class PrintingService : IPrintingService
    {
        public void PrintMasterLabel(long masterID) => Console.WriteLine($"Imprimir etiqueta master #{masterID}.");

        public void PrintJrLabel(int containerID) => Console.WriteLine($"Imprimir etiqueta Jr. #{containerID}");

        public void PrintPickingLabel(long unitID) => Console.WriteLine($"Imprimir etiqueta de picking para pieza #{unitID}");
    }
}