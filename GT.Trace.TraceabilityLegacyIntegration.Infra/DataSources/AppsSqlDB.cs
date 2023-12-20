namespace GT.Trace.TraceabilityLegacyIntegration.Infra.DataSources
{
    public class AppsSqlDB
    {
        private readonly ConfigurationSqlDbConnection<AppsSqlDB> _con;

        public AppsSqlDB(ConfigurationSqlDbConnection<AppsSqlDB> con)
        {
            _con=con;
        }

        /*De aqui para abajo se pueden poner las consultas a la base de datos*/
    }
}
