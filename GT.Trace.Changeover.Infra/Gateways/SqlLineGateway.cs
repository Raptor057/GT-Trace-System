using GT.Trace.Changeover.App.Dtos;
using GT.Trace.Changeover.App.Gateways;
using GT.Trace.Changeover.Infra.Daos;

namespace GT.Trace.Changeover.Infra.Gateways
{
    internal class SqlLineGateway : ILineGateway
    {
        private readonly LineDao _lines;

        public SqlLineGateway(LineDao lines)
        {
            _lines = lines;
        }

        public async Task<LineDto?> GetLineAsync(string lineCode)
        {
            var line = await _lines.GetLineByCode(lineCode).ConfigureAwait(false);
            if (line == null)
            {
                return null;
            }

            return new LineDto(line.id, line.letter, line.modelo.Trim(), line.active_revision.Trim(), line.codew.Trim());
        }

        public async Task UpdateWorkOrderAsync(WorkOrderDto workOrder)
        {
            await _lines.Update(workOrder.LineID, workOrder.Code, workOrder.PartNo, workOrder.Revision).ConfigureAwait(false);
        }
    }
}