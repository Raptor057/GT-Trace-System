﻿namespace GT.Trace.Common.Infra.DataSources.SqlDB
{
    /// <summary>
    /// Abstract class used to hint the connection string from the configuration file
    /// to use while connecting to the APPS database.
    /// </summary>
    public abstract class APPS
    { }

    /// <summary>
    /// Abstract class used to hint the connection string from the configuration file
    /// to use while connecting to the TRAZAB database.
    /// </summary>
    public abstract class TRAZAB
    { }

    /// <summary>
    /// Abstract class used to hint the connection string from the configuration file
    /// to use while connecting to the GTT database.
    /// </summary>
    public abstract class GTT
    { }

    /// <summary>
    /// Abstract class used to hint the connection string from the configuration file
    /// to use while connecting to the PMI database.
    /// </summary>
    public abstract class PMI
    { }

    /// <summary>
    /// Interface defining the minimum basic operations needed to work against
    /// an SQL database.
    /// </summary>
    public interface ISqlDbConnection : IDisposable
    {
        /// <summary>Executes asynchronously a select command against the database.</summary>
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? args = null);

        /// <summary>Executes asynchronously a select command against the database throwing an exception if more than one records are found.</summary>
        Task<T> QuerySingleAsync<T>(string sql, object? args = null);

        /// <summary>Executes asynchronously a select command against the database returning only the first record.</summary>
        Task<T> QueryFirstAsync<T>(string sql, object? args = null);

        /// <summary>Executes asynchronously a select command against the database returning only the first value.</summary>
        Task<T> ExecuteScalarAsync<T>(string sql, object? args = null);

        /// <summary>Executes asynchronously an SQL command against the database returning the number of affected rows.</summary>
        Task<int> ExecuteAsync(string sql, object? args = null);

        /// <summary>Executes synchronously a select command against the database.</summary>
        IEnumerable<T> Query<T>(string sql, object? args = null);

        /// <summary>Executes synchronously a select command against the database throwing an exception if more than one records are found.</summary>
        T QuerySingle<T>(string sql, object? args = null);

        /// <summary>Executes synchronously a select command against the database returning only the first record.</summary>
        T QueryFirst<T>(string sql, object? args = null);

        /// <summary>Executes synchronously a select command against the database returning only the first value.</summary>
        T ExecuteScalar<T>(string sql, object? args = null);

        /// <summary>Executes synchronously an SQL command against the database returning the number of affected rows.</summary>
        int Execute(string sql, object? args = null);
    }
}