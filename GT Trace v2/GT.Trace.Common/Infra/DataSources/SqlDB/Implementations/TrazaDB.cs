namespace GT.Trace.Common.Infra.DataSources.SqlDB.Implementations
{
    /// <summary>
    /// Wrapper around DB<> to use the parameter type explicitly for the TRAZAB database.
    /// </summary>
    internal sealed class TrazaDB : GenericDB<TRAZAB>, ITraza
    {
        public TrazaDB(GenericConfigurationSqlDbConnectionFactory<TRAZAB> connections)
            : base(connections)
        { }
    }
}