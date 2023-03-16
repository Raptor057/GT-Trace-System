namespace GT.Trace.Infra.DataSources.Sql
{
    internal class GttSqlDBConnection : DisposableDapperDatabaseConnection<GttConfigurableSqlDatabaseConnectionFactory>, IGttSqlDBConnection
    {
        public GttSqlDBConnection(GttConfigurableSqlDatabaseConnectionFactory connections)
            : base(connections)
        { }
    }
}