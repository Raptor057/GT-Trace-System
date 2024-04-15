namespace GT.Trace.ConfigurationPanel.Domain.Repositories
{
    public interface ILineRepository
    {
        Task GetActiveLinesAsync();

        Task UpdatedblidAsync(string codew, int dblid);

        Task UpdatestdpackAsync(string codew, int stdpack);

    }
}
