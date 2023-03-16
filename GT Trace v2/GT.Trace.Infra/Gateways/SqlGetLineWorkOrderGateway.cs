using GT.Trace.App.UseCases.Lines.GetWorkOrder;
using GT.Trace.Common;
using GT.Trace.Infra.Daos;

namespace GT.Trace.Infra.Gateways
{
    internal record SqlGetLineWorkOrderGateway(LineDao Lines, WorkOrderDao WorkOrders)
        : IGetWorkOrderGateway
    {
        public async Task<Result<WorkOrderDto>> GetWorkOrderAsync(string lineCode)
        {
            var workOrderCode = await WorkOrders.GetActiveWorkOrderAsync(lineCode).ConfigureAwait(false);

            var prod_unit = await Lines.GetLineByLineCodeAsync(lineCode).ConfigureAwait(false);
            if (prod_unit == null)
            {
                return Result.Fail<WorkOrderDto>($"Línea \"{lineCode}\" no encontrada.");
            }

            Entities.pro_production production;
            if (string.IsNullOrWhiteSpace(workOrderCode))
            {
                production = await WorkOrders.GetWorkOrderByCodeAsync(prod_unit.id, prod_unit.codew).ConfigureAwait(false);
            }
            else
            {
                production = await WorkOrders.GetWorkOrderByCodeAsync(workOrderCode).ConfigureAwait(false);
                prod_unit.modelo = production.part_number.Trim();
                prod_unit.active_revision = production.rev;
                prod_unit.codew = production.codew;
            }
            production.part_number = production.part_number.Trim();
            production.client_name = production.client_name.Trim();
            production.ratio = production.ratio.Trim();
            production.rev = production.rev.Trim();
            production.ref_ext = production.ref_ext.Trim();

            return Result.OK(new WorkOrderDto(production.codew, production.part_number, production.rev, production.target_qty ?? 0, production.current_qty ?? 0));
        }
    }
}