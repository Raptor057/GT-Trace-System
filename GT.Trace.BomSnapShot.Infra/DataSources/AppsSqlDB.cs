using GT.Trace.BomSnapShot.Infra.DataSources.Entities;

namespace GT.Trace.BomSnapShot.Infra.DataSources
{
    public class AppsSqlDB
    {

        private readonly DapperSqlDbConnection _con;

        public AppsSqlDB(ConfigurationSqlDbConnection<AppsSqlDB> con)
        {
            _con = con;
        }

        //Aqui se obtiene la info de la tabla pro_production que tiene las ordenes activas de GTAPP por linea
        public async Task<pro_production> GetLineProductionActiveByPointOfUseCodeAsync(string lineCode) =>
        await _con.QueryFirstAsync<pro_production>("SELECT TOP 1 PP.* FROM [MXSRVTRACA].[APPS].[dbo].[pro_prod_units] PPU " +
        "INNER JOIN [MXSRVTRACA].[APPS].[dbo].[pro_production] PP ON  PPU.id = PP.id_line AND PP.is_stoped = 0 AND PP.is_running = 1 AND PP.is_finished = 0 " +
        "WHERE ppu.letter = @lineCode", new { lineCode }).ConfigureAwait(false);


        //entrega una lista de las lineas con componentes compartidos para ver que lineas se van a actualizar
        //esto en caso con finalidad de actualizar las gamas de todas esas lineas, funciona solo si las lineas contienen una orden activa en GT-APP
        //public async Task<IEnumerable<string>> GetLinesCodesSharingPointOfUseAsync(string pointOfUseCode, string componentNo) =>
        //await _con.QueryAsync<string>("SELECT DISTINCT RTRIM(CB.NOKTCOMPF) AS [LineCode],RTRIM(CB.NOKTCODPF) AS [Model]" +
        //"FROM [MXSRVTRACA].[APPS].[dbo].[pro_prod_units] PPU " +
        //"INNER JOIN [MXSRVTRACA].[APPS].[dbo].[pro_production] PP ON  PPU.id = PP.id_line AND PP.is_stoped = 0 AND PP.is_running = 1 AND PP.is_finished = 0 " +
        //"INNER JOIN [MXSRVTRACA].[TRAZAB].[cegid].[bom] CB ON RTRIM(CB.NOKTCODPF) = RTRIM(PP.part_number) AND RTRIM(CB.NOKTCOMPF) = RTRIM(PPU.letter) " +
        //"WHERE CB.NOCTCODECP = @componentNo AND CB.NOCTCODOPE = @pointOfUseCode ", new { pointOfUseCode, componentNo }).ConfigureAwait(false);

public async Task<IEnumerable<LinesCodesShared>> GetLinesCodesSharingPointOfUseAsync(string pointOfUseCode, string componentNo)
{
    return await _con.QueryAsync<LinesCodesShared>(
        "SELECT DISTINCT RTRIM(CB.NOKTCOMPF) AS [LineCode],RTRIM(CB.NOKTCODPF) AS [Model]" +
        "FROM [MXSRVTRACA].[APPS].[dbo].[pro_prod_units] PPU " +
        "INNER JOIN [MXSRVTRACA].[APPS].[dbo].[pro_production] PP ON  PPU.id = PP.id_line AND PP.is_stoped = 0 AND PP.is_running = 1 AND PP.is_finished = 0 " +
        "INNER JOIN [MXSRVTRACA].[TRAZAB].[cegid].[bom] CB ON RTRIM(CB.NOKTCODPF) = RTRIM(PP.part_number) AND RTRIM(CB.NOKTCOMPF) = RTRIM(PPU.letter) " +
        "WHERE CB.NOCTCODECP = @componentNo AND CB.NOCTCODOPE = @pointOfUseCode ",
        new { pointOfUseCode, componentNo }
    ).ConfigureAwait(false);
}


    }
}