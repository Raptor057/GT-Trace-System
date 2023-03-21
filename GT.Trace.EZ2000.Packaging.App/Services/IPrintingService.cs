namespace GT.Trace.EZ2000.Packaging.App.Services
{
    public interface IPrintingService
    {
        void PrintMasterLabel(long masterID);
        void PrintJrLabel(int containerID);
        void PrintPickingLabel(long unitID);
    }
}
