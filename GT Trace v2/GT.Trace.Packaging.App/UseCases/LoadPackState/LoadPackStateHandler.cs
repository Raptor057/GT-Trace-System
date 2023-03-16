using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.UseCases.LoadPackState.Dtos;
using GT.Trace.Packaging.App.UseCases.LoadPackState.Responses;
using GT.Trace.Packaging.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace GT.Trace.Packaging.App.UseCases.LoadPackState
{
    internal sealed class LoadPackStateHandler : IInteractor<LoadPackStateRequest, LoadPackStateResponse>
    {
        private readonly IStationRepository _stations;

        private readonly ILogger<LoadPackStateHandler> _logger;

        public LoadPackStateHandler(IStationRepository stations, ILogger<LoadPackStateHandler> logger)
        {
            _stations = stations;
            _logger = logger;
        }

        public async Task<LoadPackStateResponse> Handle(LoadPackStateRequest request, CancellationToken cancellationToken)
        {
            var station = await _stations.GetStationByHostnameAsync(request.Hostname, request.LineCode, request.PalletSize, request.ContainerSize, request.PoNumber).ConfigureAwait(false);
            if (station == null)
            {
                return new StationNotFoundResponse(request.Hostname);
            }

            if (station.Line.IsChangeOverRequired)
            {
                return new WrongLinePartNumberResponse(station.Line.WorkOrder.Code, station.Line.LoadedPart.Number, station.Line.WorkOrder.Part.Number);
            }

            if (!station.IsPackagingProcess)
            {
                return new WrongStationProcessResponse(request.Hostname, station.ProcessName);
            }

            const string packfol = @"\\MXSRVCEGID\data2\Shared\Engineering\Traza_Empaque PT\Packing\";
            var packagingImageBase64 = string.Empty;
            var packagingImageFileName = Path.Combine(packfol, $"{station.Line.WorkOrder.Part.ProductFamily}.jpg");
            if (!File.Exists(packagingImageFileName))
            {
                packagingImageFileName = string.Empty;
            }
            else
            {
                var bytes = File.ReadAllBytes(packagingImageFileName);
                packagingImageBase64 = Convert.ToBase64String(bytes);
            }

            const string packfol2 = @"\\MXSRVCEGID\data2\Shared\Engineering\Traza_Empaque PT\";
            var sampleImageBase64 = string.Empty;
            var sampleImageFileName = Path.Combine(packfol2, $"{station.Line.WorkOrder.Part.Number}.jpg");
            if (!File.Exists(sampleImageFileName))
            {
                sampleImageFileName = Path.Combine(packfol2, "empaque-no-especificado.jpg");
            }
            if (File.Exists(sampleImageFileName))
            {
                var bytes = File.ReadAllBytes(sampleImageFileName);
                sampleImageBase64 = Convert.ToBase64String(bytes);
            }

            return new SuccessLoadPackStateResponse(new PackStateDto(
                station.Line.Name,
                station.Line.Code,
                station.Line.WorkOrder.Part.ProductFamily ?? "",
                station.Line.Headcount,
                new PartDto(
                    station.Line.WorkOrder.Part.Number,
                    station.Line.WorkOrder.Part.Revision.Number,
                    station.Line.WorkOrder.Part.Description),
                new WorkOrderDto(
                    station.Line.WorkOrder.Code,
                    station.Line.WorkOrder.Size,
                    station.Line.WorkOrder.Quantity,
                    station.Line.WorkOrder.PO.Number,
                    new MasterTypeDto(station.Line.WorkOrder.MasterType == Domain.Enums.MasterTypes.ATEQ),
                    new PackTypeDto(station.Line.Pallet.ContainerTypeIsBox),
                    new PartDto(station.Line.WorkOrder.Part.Number, station.Line.WorkOrder.Part.Revision.Number, station.Line.WorkOrder.Part.Description),
                    new ClientDto(station.Line.WorkOrder.Client.Code.ToString(), station.Line.WorkOrder.Client.Name, station.Line.WorkOrder.Client.Description, station.Line.WorkOrder.CustomerPartNo)),
                new StationDto(
                    request.Hostname,
                    station.Line.Name, station.CanTrace, station.CanPick, station.CanValidateFunctionalTest, station.CanValidateCustomerPartNo, station.CanValidateRevision),
                new PalletDto(
                    station.Line.Pallet.Size,
                    station.Line.Pallet.Quantity,
                    sampleImageBase64,
                    packagingImageBase64,
                    new ContainerDto(station.Line.Pallet.GetActiveContainer()?.Size ?? 0, station.Line.Pallet.GetActiveContainer()?.Quantity ?? 0),
                    station.Line.Pallet.Approval?.IsApproved ?? false),
                new BomLabelDto(station.Line.Bom.Label.Component, $"{station.Line.Bom.Label.Component}: {station.Line.Bom.Label.Description}"),
                new ApprovalDto(station.Line.QcUnapprovedPalletWarnLimit, station.Line.QcUnapprovedPalletStopLimit),
                new PickingDto(station.Line.Picking.IsActive ? 0 : station.Line.Picking.Period - station.Line.Picking.Counter, station.Line.Picking.Sequence, station.Line.Picking.TotalSamples, station.Line.Picking.Period),
                station.Line.HourlyProduction.Select(item => new HourlyProductionItemDto(item.IntervalName, item.IsPastDue, item.IsCurrent, item.PartNo, item.TargetQuantity, item.Quantity))));
        }
    }
}