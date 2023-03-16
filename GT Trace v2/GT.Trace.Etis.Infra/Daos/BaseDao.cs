using GT.Trace.Common.Infra;

namespace GT.Trace.Etis.Infra.Daos
{
    internal abstract class BaseDao
    {
        protected BaseDao(ISqlDatabaseConnection connection)
        {
            Connection = connection;
        }

        protected ISqlDatabaseConnection Connection { get; }
    }
}