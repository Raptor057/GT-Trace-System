namespace GT.Trace.Common.Infra.DataSources.SqlDB.Implementations
{
    /// <summary>
    /// Wrapper around DB<> to use the parameter type explicitly for the APPS database.
    /// </summary>
    internal sealed class AppsDB : GenericDB<APPS>, IApps
    {
        public AppsDB(GenericConfigurationSqlDbConnectionFactory<APPS> connections)
            : base(connections)
        { }
    }
}