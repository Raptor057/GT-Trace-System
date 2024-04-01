namespace GT.Trace.TraceabilityLegacyIntegration.Infra.DataSources
{
    public class AppsSqlDB
    {
        private readonly DapperSqlDbConnection _con;

        public AppsSqlDB(ConfigurationSqlDbConnection<AppsSqlDB> con)
        {
            _con = con;
        }
    }
}
