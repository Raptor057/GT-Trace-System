namespace GT.Trace.Common.Infra.DataSources.SqlDB.Implementations
{
    /// <summary>
    /// Generic class that uses the parameter type to hint the connection string from the configuration
    /// file to use while connecting to database.
    /// </summary>
    internal class GenericDB<T> : DapperSqlDbConnection, IGenericDB<T>
    {
        public GenericDB(GenericConfigurationSqlDbConnectionFactory<T> connections)
            : base(connections.CreateOpenConnection())
        { }
    }
}