namespace GT.Trace.TraceabilityLegacyIntegration.Infra.DataSources
{
    public class CegidSqlDB
    {
        private readonly ConfigurationSqlDbConnection<CegidSqlDB> _con;

        public CegidSqlDB(ConfigurationSqlDbConnection<CegidSqlDB> con)
        {
            _con = con;
        }

        /*De aqui para abajo se pueden poner las consultas a la DB*/

    }
}
