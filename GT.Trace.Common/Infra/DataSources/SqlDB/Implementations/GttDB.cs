namespace GT.Trace.Common.Infra.DataSources.SqlDB.Implementations
{
    /// <summary>
    /// Wrapper around DB<> to use the parameter type explicitly for the GTT database.
    /// </summary>
    internal sealed class GttDB : GenericDB<GTT>, IGtt
    {
        public GttDB(GenericConfigurationSqlDbConnectionFactory<GTT> connections)
            : base(connections)
        { }
    }
}