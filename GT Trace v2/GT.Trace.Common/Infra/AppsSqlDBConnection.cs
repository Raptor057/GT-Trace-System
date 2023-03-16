namespace GT.Trace.Common.Infra
{
    public class AppsSqlDBConnection : DisposableDapperDatabaseConnection<AppsConfigurableSqlDatabaseConnectionFactory>, IAppsSqlDBConnection
    {
        public AppsSqlDBConnection(AppsConfigurableSqlDatabaseConnectionFactory connections)
            : base(connections)
        { }
    }
}