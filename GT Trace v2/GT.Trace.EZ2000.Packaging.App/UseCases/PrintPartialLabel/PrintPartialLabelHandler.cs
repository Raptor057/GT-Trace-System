using GT.Trace.Common.CleanArch;
using GT.Trace.EZ2000.Packaging.App.Dtos;
using GT.Trace.EZ2000.Packaging.App.Gateways;
using GT.Trace.EZ2000.Packaging.App.UseCases.PrintPartialLabel.Responses;
using GT.Trace.EZ2000.Packaging.Domain.Entities;
using GT.Trace.EZ2000.Packaging.Domain.Repositories;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.PrintPartialLabel
{
    internal sealed class PrintPartialLabelHandler : IInteractor<PrintPartialLabelRequest, PrintPartialLabelResponse>
    {
        private readonly IStationRepository _stations;

        private readonly IMasterLabelsGateway _masterLabels;

        public PrintPartialLabelHandler(IStationRepository stations, IMasterLabelsGateway masterLabels)
        {
            _stations = stations;
            _masterLabels = masterLabels;
        }

        public async Task<PrintPartialLabelResponse> Handle(PrintPartialLabelRequest request, CancellationToken cancellationToken)
        {
            Station station = await _stations.GetStationByHostnameAsync(request.Hostname, null, null, null, null).ConfigureAwait(false);
            if (station == null)
            {
                return new StationNotFoundResponse(request.Hostname);
            }

            if (!station.CanResetPartialContainer(out var errors))
            {
                return new ValidationFailureResponse(errors);
            }

            var palletQuantity = station.Line.Pallet.Quantity;
            station.ResetPartialContainer();

            await _stations.SaveAsync(station).ConfigureAwait(false);

            var masterLabelID = await _masterLabels.GetLastMasterFolioByLineAsync(station.Line.Name).ConfigureAwait(false);

            var date = DateTime.Now.Date;
            return new SuccessPrintPartialLabelResponse(station.Line.Code, new PalletDto
            {
                ApprovalDate = station.Line.Pallet.Approval?.Date,
                Approver = station.Line.Pallet.Approval?.Username,
                Customer = station.Line.WorkOrder.Client.Description,
                CustomerPartNo = station.Line.WorkOrder.CustomerPartNo,
                Date = date,
                IsAteq = false, //station.Line.WorkOrder.MasterType == Domain.Enums.MasterTypes.ATEQ,
                IsPartial = true,
                JulianDate = $"{date.DayOfYear:000}{date.Year - 2000}",
                LineName = station.Line.Name,
                MasterID = masterLabelID,
                PartDescription = station.Line.WorkOrder.Part!.Description,
                PartNo = station.Line.WorkOrder.Part!.Number,
                PurchaseOrderNo = station.Line.WorkOrder.PO.Number,
                Quantity = palletQuantity,
                Revision = station.Line.WorkOrder.Part.Revision.OriginalValue
            });
        }
    }
}