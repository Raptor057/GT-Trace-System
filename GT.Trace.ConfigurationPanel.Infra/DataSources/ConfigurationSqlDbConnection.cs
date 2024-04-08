namespace GT.Trace.ConfigurationPanel.Infra.DataSources
{
    public class ConfigurationSqlDbConnection<T> : DapperSqlDbConnection
    {
        public ConfigurationSqlDbConnection(ConfigurationSqlDbConnectionFactory<T> factory)
            : base(factory)
        { }
    }
}
