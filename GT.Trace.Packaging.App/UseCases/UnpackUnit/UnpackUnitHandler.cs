using GT.Trace.Common.CleanArch;
using GT.Trace.Common.Infra;
using GT.Trace.Packaging.App.Services;
using GT.Trace.Packaging.Domain.Entities;
using GT.Trace.Packaging.Domain.Repositories;
using System.Text.RegularExpressions;

namespace GT.Trace.Packaging.App.UseCases.UnpackUnit
{
    internal sealed class UnpackUnitHandler : IInteractor<UnpackUnitRequest, UnpackUnitResponse>
    {
        private readonly ILabelParserService _labelParser;

        private readonly IStationRepository _stations;

        private readonly IUnitRepository _units;

        private readonly IUnpackUnitGateway _unpackUnit;

        public UnpackUnitHandler(ILabelParserService labelParser, IStationRepository stations, IUnitRepository units, IUnpackUnitGateway unpackUnit)
        {
            _labelParser = labelParser;
            _stations = stations;
            _units = units;
            _unpackUnit=unpackUnit;
        }

        public async Task<UnpackUnitResponse> Handle(UnpackUnitRequest request, CancellationToken cancellationToken)
        {
            long unitID;

            //const string pattern = @"^.+\|.+\|(?<datetime>.+)\|(?<serial>.{11})$";
            //const string pattern2 = @"^\s*(\d+)\s+(\w)\s+(\d{4}-\d{2}-\d{2})\s+(\d{2}:\d{2}:\d{2})\s+(\d+\.\d+[A-Za-z])\s+(\d+)\s+(\d+)\s*$"; // RA 02/22/2024: Esto se agrego para leer el QR de motores de las lineas MW Y MX Rev.2
            const string pattern2 = @"^\s*([A-Za-z0-9]+)\s+(\w)\s+(\d{4}-\d{2}-\d{2})\s+(\d{2}:\d{2}:\d{2})\s+(\d+\.\d+[A-Za-z])\s+(\d+)\s+(\d+)\s*$"; // RA 02/22/2024: Esto se agrego para leer el QR de motores de las lineas MW Y MX Rev.2
            //var match = Regex.Match(request.ScanInput ?? "", pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var match2 = Regex.Match(request.ScanInput ?? "", pattern2, RegexOptions.Singleline | RegexOptions.IgnoreCase); // RA 11:14/2023: Esto se agrego para leer el QR de motores de las lineas MW Y MX
            
            //Comentado debido a que estas lineas MU y MV ya no van a correr pero se mantiene como historico
            //if (match.Success)
            //{
            //    var creationTime = DateTime.ParseExact(match.Groups["datetime"].Value, "yyMMdd-HHmmss", System.Globalization.CultureInfo.InvariantCulture);
            //    var serialCode = match.Groups["serial"].Value;

            //    var id = await _units.GetUnitIDBySerialCodeAsync(serialCode).ConfigureAwait(false);
            //    if (!id.HasValue)
            //    {
            //        unitID = await _units.AddUnitAsync(request.LineCode, 0, serialCode, creationTime).ConfigureAwait(false);
            //    }
            //    else unitID = id.Value;
            //}
            //else 
            if (match2.Success)
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
                unitID = id.Value;
                _ = _unpackUnit.UnpackedUnitAsync(request.LineName, unitID, request.WorkOrderCode, request.LineCode).ConfigureAwait(false);
            }
            else
            {
                if (!_labelParser.TryParseNewWBFormat(request.ScanInput ?? "", out var label) || label == null)
                {
                    return new FailureUnpackUnitResponse($"Ocurrió un problema al procesar la entrada \"{request.ScanInput}\"");
                }

                unitID = label.UnitID;
                _ = _unpackUnit.UnpackedUnitAsync(request.LineName, unitID, request.WorkOrderCode, request.LineCode).ConfigureAwait(false);
            }

            return new SuccessUnpackUnitResponse(request.LineCode, unitID, request.LineName, request.WorkOrderCode);
        }
    }
}