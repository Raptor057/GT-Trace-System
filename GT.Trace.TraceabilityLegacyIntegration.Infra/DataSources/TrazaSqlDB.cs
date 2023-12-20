namespace GT.Trace.TraceabilityLegacyIntegration.Infra.DataSources
{
    public class TrazaSqlDB
    {
        private readonly ConfigurationSqlDbConnection<TrazaSqlDB> _con;

        public TrazaSqlDB(ConfigurationSqlDbConnection<TrazaSqlDB> con)
        {
            _con = con;
        }
        /*De aqui para abajo se pueden poner todas las consultas necesarias*/
    }
}
