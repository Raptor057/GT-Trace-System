using GT.Trace.Common.Infra;

namespace GT.Trace.Infra.Daos
{
    internal class SubAssemblyDao : BaseDao
    {
        public SubAssemblyDao(IGttSqlDBConnection connection)
            : base(connection)
        { }

        //aqui se agregan los sub ensambles creados.
        //pero esto solo es un registro de los sub ensambles ya que no se envia la info de los sub ensambles al servidor de impresion RA: 05/18/2023
        public async Task<long> AddSubAssemblyAsync(string lineCode, string componentNo, string revision, string workOrderCode, int quantity) =>
            await Connection.ExecuteScalarAsync<long>(
                "INSERT INTO dbo.SubAssemblies(LineCode, ComponentNo, Revision, WorkOrderCode, Quantity) VALUES(@lineCode, @componentNo, @revision, @workOrderCode, @quantity); SELECT SCOPE_IDENTITY();",
                new { lineCode, componentNo, revision, workOrderCode, quantity })
            .ConfigureAwait(false);
    }
}