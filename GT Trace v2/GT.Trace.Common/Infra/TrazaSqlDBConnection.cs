namespace GT.Trace.Common.Infra
{
    public class TrazaSqlDBConnection : DisposableDapperDatabaseConnection<TrazaConfigurableSqlDatabaseConnectionFactory>, ITrazaSqlDBConnection
    {
        public TrazaSqlDBConnection(TrazaConfigurableSqlDatabaseConnectionFactory connections)
            : base(connections)
        { }
    }
}