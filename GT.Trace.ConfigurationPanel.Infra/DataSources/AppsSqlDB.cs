using GT.Trace.ConfigurationPanel.Domain.Entities;

namespace GT.Trace.ConfigurationPanel.Infra.DataSources
{
    public class AppsSqlDB
    {
        private readonly DapperSqlDbConnection _con;
        public AppsSqlDB(ConfigurationSqlDbConnection<AppsSqlDB> con)
        {
            _con = con;
        }

        /// <summary>
        /// Consulta las lineas activas en base a las condiciones
        /// is_stoped = 0 AND is_running = 1 AND is_finished = 0 en la tabla [APPS].[dbo].[pro_production]
        /// </summary>
        /// <returns>
        /// Regresa las lineas activas en GT-APP
        /// </returns>
        public async Task<IEnumerable<ActiveLines>> GetActiveLinesAsync()=>
            await _con.QueryAsync<ActiveLines>("SELECT PP.id_line,PPU.[letter],PPU.[comments],PP.[codew],PP.[part_number],PP.[rev],PP.[part_desc],PP.[std_pack],PP.[ref_ext],PP.[client_code],PP.[client_name],PP.[dblid] FROM [APPS].[dbo].[pro_prod_units] PPU INNER JOIN [APPS].[dbo].[pro_production] PP ON PPU.id = PP.id_line AND pp.is_stoped = 0 AND pp.is_running = 1 AND pp.is_finished = 0;", new { }).ConfigureAwait(false);

        /// <summary>
        /// Actualiza la cantidad de estandar pack
        /// </summary>
        /// <param name="codew">
        /// </param>
        /// <param name="stdpack"></param>
        /// <returns>
        /// </returns>
        public async Task UpdatestdpackAsync(string codew, int stdpack) =>
            await _con.ExecuteAsync("UPDATE pro_production SET std_pack = @stdpack WHERE is_stoped = 0 AND is_running = 1 AND is_finished = 0 AND codew = @codew;", new { codew , stdpack }).ConfigureAwait(false);

        /// <summary>
        /// Actualiza la cantidad de etiquetas a imprimir
        /// </summary>
        /// <param name="codew"></param>
        /// <param name="dblid"></param>
        /// <returns>
        /// </returns>
        public async Task UpdatedblidAsync(string codew, int dblid) =>
            await _con.ExecuteAsync("UPDATE pro_production SET dblid = @dblid WHERE is_stoped = 0 AND is_running = 1 AND is_finished = 0 AND codew = @codew;", new { codew, dblid }).ConfigureAwait(false);
    }
}
