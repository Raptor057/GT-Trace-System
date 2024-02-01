namespace GT.Trace.BomSnapShot.Infra.DataSources
{
    public class TrazaSqlDB
    {
        private readonly DapperSqlDbConnection _con;

        public TrazaSqlDB(ConfigurationSqlDbConnection<TrazaSqlDB> con)
        {
            _con = con;
        }

        //Cuenta la cantidad de componentes segun la gama entregando el numero de parte y la linea
        public async Task<int> BomComponentCountByLineCodeAndPartNoAsync(string lineCode, string partNo) =>
        await _con.QueryFirstAsync<int>("SELECT COUNT([NOKTCODPF]) AS [BomComponentCount] FROM [TRAZAB].[cegid].[bom] WHERE NOKTCOMPF = @lineCode AND NOKTCODPF = @partNo", new { lineCode, partNo }).ConfigureAwait(false);
    }
}