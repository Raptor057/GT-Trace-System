using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.Dtos;
using GT.Trace.Packaging.Domain.Entities;
using GT.Trace.Packaging.Domain.Repositories;

namespace GT.Trace.Packaging.App.UseCases.PrintWipLabel
{
    internal sealed class PrintWipLabelHandler : IInteractor<PrintWipLabelRequest, PrintWipLabelResponse>
    {
        private readonly IStationRepository _stations;

        public PrintWipLabelHandler(IStationRepository stations)
        {
            _stations = stations;
        }

        public async Task<PrintWipLabelResponse> Handle(PrintWipLabelRequest request, CancellationToken cancellationToken)
        {
            Station station = await _stations.GetStationByHostnameAsync(request.HostName, null, null, null, null).ConfigureAwait(false);
            if (station == null)
            {
                return new PrintWipLabelFailureResponse($"Estación \"{request.HostName}\" no encontrada.");
            }

            var date = DateTime.Now.Date;
            return new PrintWipLabelSuccessResponse(station.Line.Code, new PalletDto
            {
                ApprovalDate = station.Line.Pallet.Approval?.Date,
                Approver = station.Line.Pallet.Approval?.Username,
                Customer = station.Line.WorkOrder.Client.Description,
                CustomerPartNo = station.Line.WorkOrder.CustomerPartNo,
                Date = date,
                // TODO: I'm not sure about this... I decided to left it false as this is for a WIP label.
                IsAteq = false, //station.Line.WorkOrder.MasterType == Domain.Enums.MasterTypes.ATEQ,
                IsPartial = false,
                JulianDate = $"{date.DayOfYear:000}{date.Year - 2000}",
                LineName = station.Line.Name,
                MasterID = null,
                PartDescription = station.Line.WorkOrder.Part!.Description,
                PartNo = station.Line.WorkOrder.Part!.Number,
                PurchaseOrderNo = station.Line.WorkOrder.PO.Number,
                Quantity = station.Line.Pallet.Quantity,
                Revision = station.Line.WorkOrder.Part.Revision.OriginalValue
            });
        }
    }
}