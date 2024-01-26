namespace GT.Trace.BomSnapShot.Infra.DataSources
{
    public class TrazaSqlDB
    {
        private readonly DapperSqlDbConnection _con;

        public TrazaSqlDB(ConfigurationSqlDbConnection<TrazaSqlDB> con)
        {
            _con = con;
        }
    }
}