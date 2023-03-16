using GT.Trace.Packaging.App.UseCases.SetStationBlocked;
using GT.Trace.Packaging.Infra.DataSources;

namespace GT.Trace.Packaging.Infra.Repositories
{
    //Esto dejalo asi, es para en un furuto saldar la deuda tecnica, pero de momento no le piques
    //RAG 2/09/2023

    //internal class SetStationBlockedRepository : ISetStationBlockedGateway
    //{
    //    private readonly TrazaSqlDB _traza;

    //    public SetStationBlockedRepository(TrazaSqlDB traza)
    //    {
    //        _traza = traza;
    //    }


    //    /// <summary>
    //    /// In theory, this method blocks the line.
    //    /// Execute the query in TrazaSqlDB with name "SetStationBlocked".
    //    /// </summary>
    //    /// <param name="is_blocked"></param>
    //    /// <param name="lineName"></param>
    //    /// <returns></returns>
    //    public async Task StationBlocked(string is_blocked, string lineName)
    //    {
    //        var StationBlocked = await _traza.SetStationBlocked(is_blocked, lineName).ConfigureAwait(false);
    //    }

    //}
}
