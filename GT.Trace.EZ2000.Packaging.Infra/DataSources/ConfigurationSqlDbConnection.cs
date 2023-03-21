namespace GT.Trace.EZ2000.Packaging.Infra.DataSources
{
    public class ConfigurationSqlDbConnection<T> : DapperSqlDbConnection
    {
        public ConfigurationSqlDbConnection(ConfigurationSqlDbConnectionFactory<T> factory)
            : base(factory)
        { }
    }
}