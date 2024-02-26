using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.Dtos;
using GT.Trace.Packaging.App.Services;
using GT.Trace.Packaging.App.UseCases.PackUnit.Responses;
using GT.Trace.Packaging.Domain.Entities;
using GT.Trace.Packaging.Domain.Repositories;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GT.Trace.Packaging.App.UseCases.PackUnit
{
    internal sealed class PackUnitHandler : IInteractor<PackUnitRequest, PackUnitResponse>
    {
        private readonly ILabelParserService _labelParser;

        private readonly IStationRepository _stations;

        private readonly IUnitRepository _units;

        public PackUnitHandler(ILabelParserService labelParser, IStationRepository stations, IUnitRepository units)
        {
            _labelParser = labelParser;
            _stations = stations;
            _units = units;
        }

        public async Task<PackUnitResponse> Handle(PackUnitRequest request, CancellationToken cancellationToken)
        {
            Station station = await _stations.GetStationByHostnameAsync(request.Hostname, request.LineCode, request.PalletSize, request.ContainerSize, request.PoNumber).ConfigureAwait(false);
            if (station == null)
            {
                return new StationNotFoundResponse(request.Hostname);
            }

            long unitID;

            const string pattern = @"^.+\|.+\|(?<datetime>.+)\|(?<serial>.{1,})$";
            //const string pattern2 = @"^\s*(\d{2}\.\d{2}[A-Za-z])\s+(\d+)\s+(\w+)\s+(\w+)\s+(\d{4}-\d{2}-\d{2})\s+(\d{2}:\d{2}:\d{2})\s+(\d+)\s*$"; // RA 11:14/2023: Esto se agrego para leer el QR de motores de las lineas MW Y MX
            const string pattern2 = @"^\s*(\d+)\s+(\w)\s+(\d{4}-\d{2}-\d{2})\s+(\d{2}:\d{2}:\d{2})\s+(\d+\.\d+[A-Za-z])\s+(\d+)\s+(\d+)\s*$"; // RA 02/22/2024: Esto se agrego para leer el QR de motores de las lineas MW Y MX Rev.2
            //const string pattern2 = @"^\s*(?<modelo>\d+)\s+(?<rev>\w)\s+(?<fecha>\d{4}-\d{2}-\d{2})\s+(?<hora>\d{2}:\d{2}:\d{2})\s+(?<volt>\d+\.\d+[A-Za-z])\s+(?<rpm>\d+)\s+(?<serial>\d+)\s*$"; // RA 02/22/2024: Esto se agrego para leer el QR de motores de las lineas MW Y MX Rev.2
            var match = Regex.Match(request.ScannerInput ?? "", pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var match2 = Regex.Match(request.ScannerInput ?? "", pattern2, RegexOptions.Singleline | RegexOptions.IgnoreCase); // RA 11:14/2023: Esto se agrego para leer el QR de motores de las lineas MW Y MX
            if (match.Success)
            {
                var creationTime = DateTime.ParseExact(match.Groups["datetime"].Value, "yyMMdd-HHmmss", System.Globalization.CultureInfo.InvariantCulture);
                var serialCode = match.Groups["serial"].Value;

                var id = await _units.GetUnitIDBySerialCodeAsync(serialCode).ConfigureAwait(false);
                if (!id.HasValue)
                {
                    unitID = await _units.AddUnitAsync(station.Line.Code, 0, serialCode, creationTime).ConfigureAwait(false);
                }
                else unitID = id.Value;
            }
            #region RA 11:14/2023: Esto se agrego para leer el QR de motores de las lineas MW Y MX
            else if (match2.Success)
            {
                string volt = match2.Groups[5].Value;
                string rpm = match2.Groups[6].Value;
                string modelo = match2.Groups[1].Value;
                string rev = match2.Groups[2].Value;
                string fechaStr = match2.Groups[3].Value;
                string horaStr = match2.Groups[4].Value;
                string serialCode = match2.Groups[7].Value;


                // Convertir fecha y hora a objetos DateTime
                DateTime fecha = DateTime.ParseExact(fechaStr, "yyyy-MM-dd", null);
                DateTime hora = DateTime.ParseExact(horaStr, "HH:mm:ss", null);

                // Combinar fecha y hora en un solo objeto DateTime
                DateTime creationTime = fecha.Add(hora.TimeOfDay);
                long valorEntero = long.Parse(creationTime.ToString("yyyyMMddHHmmss"));

                //convina fecha y hora con serial para sacar un valor unico.
                serialCode = valorEntero + serialCode;

                var id = await _units.GetUnitIDBySerialCodeAsync(serialCode).ConfigureAwait(false);
                await _units.AddMotorsDataAsync(serialCode, modelo, volt, rpm, creationTime, rev).ConfigureAwait(false);
                if (!id.HasValue)
                {
                    unitID = await _units.AddUnitAsync(station.Line.Code, 0, serialCode, creationTime).ConfigureAwait(false);
                    
                }
                else unitID = id.Value;

            }
            #endregion
            else
            {
                if (!_labelParser.TryParseNewWBFormat(request.ScannerInput ?? "", out var label) || label == null)
                {
                    return new IncorrectLabelFormatResponse(request.ScannerInput);
                }

                if (!station.ValidateLabel(label, out var labelValidationErrors))
                {
                    return new InvalidLabelResponse(labelValidationErrors);
                }

                unitID = label.UnitID;

                #region
                /*Agregado nuevo para LE para modelos EZ
                 12/04/2023
                */
                string[] EZPartNo = { "87245", "87244", "87248" };
                var PartNo = await _stations.GetPartNoAsync(station.Line.Code).ConfigureAwait(false);
                bool IsEZPartNo = EZPartNo.Contains(PartNo);

                if (IsEZPartNo)
                {
                    //bool hisroty = await _stations.GetProcessHistoryAsync(unitID).ConfigureAwait(false);
                    //bool testResult = await _stations.GetFuncionalTestResultAsync(unitID).ConfigureAwait(false);
                    //if (!(testResult || hisroty))
                    //{
                    //    return new UnitNotFoundResponse(unitID);
                    //}

                    if (!(await _stations.GetProcessHistoryAsync(unitID).ConfigureAwait(false) || await _stations.GetFuncionalTestResultAsync(unitID).ConfigureAwait(false)))
                    {
                        return new UnitNotFoundResponse(unitID);
                    }
                }
                #endregion
            }

            Unit? unit = await _units.GetUnitByIDAsync(unitID).ConfigureAwait(false);
            if (unit == null)
            {
                return new UnitNotFoundResponse(unitID);
            }

            station.TryCreateQcApprovalRecord();

            if (!station.CanPackUnit(unit, out var canPackUnitErrors))
            {
                return new UnitCanNotBePackedResponse(canPackUnitErrors);
            }

            if (station.TryPickUnit(unit))
            {
                await _stations.SaveAsync(station).ConfigureAwait(false);
                return new UnitPickedResponse(station.Line.Code, unit.ID);
            }

            station.PackUnit(unit);

            var palletQuantity = station.Line.Pallet.Quantity;
            var containerQuantity = station.Line.Pallet.GetActiveContainer()!.Quantity;

            var printJrLabel = station.TryResetJrContainerIfFull();

            var printMasterLabel = station.TryResetMasterContainerIfFull();

            await _stations.SaveAsync(station).ConfigureAwait(false);

            if (printMasterLabel)
            {
                long? masterLabelID = await _stations.GetLatestMasterLabelFolioByLineAsync(station.Line.Name).ConfigureAwait(false);
                //03/30/2023: RA: Aqui se va a agregar el Origen segun el tipo de producto.
                string? origen = (await _stations.GetOrigenByCegid(station.Line.WorkOrder.Part!.Number, station.Line.WorkOrder.Part.Revision.OriginalValue).ConfigureAwait(false)) ?? " ";
                var date = DateTime.Now.Date;
                var pallet = new PalletDto
                {
                    ApprovalDate = station.Line.Pallet.Approval?.Date,
                    Approver = station.Line.Pallet.Approval?.Username,
                    Customer = station.Line.WorkOrder.Client.Description,
                    CustomerPartNo = station.Line.WorkOrder.CustomerPartNo,
                    Date = date,
                    IsAteq = station.Line.WorkOrder.MasterType == Domain.Enums.MasterTypes.ATEQ,
                    IsPartial = false,
                    JulianDate = $"{date.DayOfYear:000}{date.Year - 2000}",
                    LineName = station.Line.Name,
                    MasterID = masterLabelID,
                    PartDescription = station.Line.WorkOrder.Part!.Description,
                    PartNo = station.Line.WorkOrder.Part!.Number,
                    PurchaseOrderNo = station.Line.WorkOrder.PO.Number,
                    Quantity = palletQuantity,
                    Revision = station.Line.WorkOrder.Part.Revision.OriginalValue,
                    Origen = origen
                };
                return new PalletCompleteResponse(station.Line.Code, unit.ID, station.Line.QcContainerApprovalInWarning, station.Line.QcContainerApprovalIsRequired, pallet, station.Line.WorkOrder.Code); //Aqui se envia el dato Origen
            }
            if (printJrLabel) //Este IF se movio debido a que no se imprimia la ultima Etiqueta JR al terminar la tarima.
            {
                var date = DateTime.Now.Date;
                var container = new ContainerDto
                {
                    ApprovalDate = station.Line.Pallet.Approval?.Date,
                    Approver = station.Line.Pallet.Approval?.Username,
                    Customer = station.Line.WorkOrder.Client.Description,
                    CustomerPartNo = station.Line.WorkOrder.CustomerPartNo,
                    Date = date,
                    JulianDate = $"{date.DayOfYear:000}{date.Year - 2000}",
                    LineName = station.Line.Name,
                    PartDescription = station.Line.WorkOrder.Part!.Description,
                    PartNo = station.Line.WorkOrder.Part!.Number,
                    PurchaseOrderNo = station.Line.WorkOrder.PO.Number,
                    Quantity = containerQuantity,
                    Revision = station.Line.WorkOrder.Part.Revision.OriginalValue
                };
                return new ContainerCompleteResponse(station.Line.Code, unit.ID, station.Line.QcContainerApprovalInWarning, station.Line.QcContainerApprovalIsRequired, container, station.Line.WorkOrder.Code);
            }
            return new UnitPackedResponse(station.Line.Code, unit.ID, station.Line.QcContainerApprovalInWarning, station.Line.QcContainerApprovalIsRequired, station.Line.WorkOrder.Part.Number, station.Line.WorkOrder.Code);
        }
    }
}
