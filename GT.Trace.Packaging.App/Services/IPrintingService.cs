namespace GT.Trace.Packaging.App.Services
{
    public interface IPrintingService
    {
        void PrintMasterLabel(long masterID);

        void PrintJrLabel(int containerID);

        void PrintPickingLabel(long unitID);
    }
}
