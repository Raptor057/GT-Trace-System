namespace GT.Trace.ConfigurationPanel.Infra.DataSources
{
    public class DapperSqlDbConnection
    {
        protected readonly SqlDbConnectionFactory _connections;
        public DapperSqlDbConnection(SqlDbConnectionFactory connections)
        {
            _connections = connections;
        }
    }
}
