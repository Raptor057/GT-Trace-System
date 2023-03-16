namespace GT.Trace.Infra.DataSources.Sql
{
    internal class AppsSqlDBConnection : DisposableDapperDatabaseConnection<AppsConfigurableSqlDatabaseConnectionFactory>, IAppsSqlDBConnection
    {
        public AppsSqlDBConnection(AppsConfigurableSqlDatabaseConnectionFactory connections)
            : base(connections)
        { }
    }
}