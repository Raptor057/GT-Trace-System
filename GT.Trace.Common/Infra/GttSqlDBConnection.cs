namespace GT.Trace.Common.Infra
{
    public class GttSqlDBConnection : DisposableDapperDatabaseConnection<GttConfigurableSqlDatabaseConnectionFactory>, IGttSqlDBConnection
    {
        public GttSqlDBConnection(GttConfigurableSqlDatabaseConnectionFactory connections)
            : base(connections)
        { }
    }
}