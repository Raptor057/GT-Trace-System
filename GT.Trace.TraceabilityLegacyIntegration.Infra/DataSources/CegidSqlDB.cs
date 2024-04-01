namespace GT.Trace.TraceabilityLegacyIntegration.Infra.DataSources
{
    public class CegidSqlDB
    {
        private readonly DapperSqlDbConnection _con;

        public CegidSqlDB(ConfigurationSqlDbConnection<CegidSqlDB> con)
        {
            _con = con;
        }

    }
}
