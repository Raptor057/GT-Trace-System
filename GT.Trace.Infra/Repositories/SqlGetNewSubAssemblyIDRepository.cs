using GT.Trace.App.UseCases.Lines.GetNewSubAssemblyID;
using GT.Trace.Infra.Daos;
using System.Transactions;

namespace GT.Trace.Infra.Repositories
{
    internal class SqlGetNewSubAssemblyIDRepository : IGetNewSubAssemblyIDRepository
    {
        private readonly SubAssemblyDao _subAssemblies;

        private readonly ProductionDao _production;

        public SqlGetNewSubAssemblyIDRepository(SubAssemblyDao subAssemblies, ProductionDao production)
        {
            _subAssemblies = subAssemblies;
            _production = production;
        }

        public async Task<long> ExecuteAsync(string lineCode, string componentNo, string revision, string workOrderCode, int quantity)
        {
            long id;
            using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await _production.AddNewProductionLogAsync(new Entities.ProductionLogs
                {
                    LineCode = lineCode,
                    PartNo = componentNo,
                    Revision = revision,
                    WorkOrderCode = workOrderCode,
                    Quantity = quantity,
                    UtcTimeStamp = DateTime.UtcNow
                });
                id = await _subAssemblies.AddSubAssemblyAsync(lineCode, componentNo, revision, workOrderCode, quantity).ConfigureAwait(false);

                tx.Complete();
            }
            return id;
        }
    }
}