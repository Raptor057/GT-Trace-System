namespace GT.Trace.Common.Infra.DataSources.SqlDB.Implementations
{
    /// <summary>
    /// Wrapper around DB<> to use the parameter type explicitly for the PMI (CEGID) database.
    /// </summary>
    internal sealed class PmiDB : GenericDB<PMI>, IPmi
    {
        public PmiDB(GenericConfigurationSqlDbConnectionFactory<PMI> connections)
            : base(connections)
        { }
    }
}