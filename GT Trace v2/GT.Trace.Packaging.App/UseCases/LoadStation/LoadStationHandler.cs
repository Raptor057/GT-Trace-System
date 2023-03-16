using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.Domain.Repositories;

namespace GT.Trace.Packaging.App.UseCases.LoadStation
{
    internal sealed class LoadStationHandler : ResultInteractorBase<LoadStationRequest, LoadStationResponse>
    {
        private readonly IStationRepository _stations;

        public LoadStationHandler(IStationRepository stations)
        {
            _stations = stations;
        }

        public override async Task<Result<LoadStationResponse>> Handle(LoadStationRequest request, CancellationToken cancellationToken)
        {
            var station = await _stations.GetStationByHostnameAsync(request.Hostname, request.LineCode, request.PalletSize, request.ContainerSize, request.PoNumber).ConfigureAwait(false);
            if (station == null)
            {
                return Fail($"Estación con nombre {request.Hostname} no encontrada.");
            }

            if (station.Line.IsChangeOverRequired)
            {
                return Fail($"El modelo cargado en la línea no corresponde con el de la orden de fabricación [{station.Line.WorkOrder.Code}].\nSe requiere cambio de modelo: {station.Line.LoadedPart} -> {station.Line.WorkOrder.Part}.");
            }

            if (!station.IsPackagingProcess)
            {
                return Fail($"El proceso de la estación {request.Hostname} [{station.ProcessName}] no es empaque.");
            }

            return OK(new LoadStationResponse(new StationDto(
                request.Hostname,
                station.Line.Code,
                station.Line.Name,
                station.ProcessName,
                station.IsLocked,
                station.CanTrace,
                station.CanChangeLine,
                station.CanAutoload,
                station.CanPick,
                station.CanValidateFunctionalTest,
                station.CanValidateCustomerPartNo,
                station.CanValidateRevision,
                station.Line.WorkOrder.Code,
                station.Line.WorkOrder.Part.Number)));
        }
    }
}