using GT.Trace.Common.CleanArch;
using GT.Trace.EZ2000.Packaging.App.Services;
using GT.Trace.EZ2000.Packaging.Domain.Repositories;
using System.Text.RegularExpressions;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.UnpackUnit
{
    internal sealed class UnpackUnitHandler : IInteractor<UnpackUnitRequest, UnpackUnitResponse>
    {
        private readonly ILabelParserService _labelParser;

        private readonly IStationRepository _stations;

        private readonly IUnitRepository _units;

        public UnpackUnitHandler(ILabelParserService labelParser, IStationRepository stations, IUnitRepository units)
        {
            _labelParser = labelParser;
            _stations = stations;
            _units = units;
        }

        public async Task<UnpackUnitResponse> Handle(UnpackUnitRequest request, CancellationToken cancellationToken)
        {
            long unitID;

            const string pattern = @"^.+\|.+\|(?<datetime>.+)\|(?<serial>.{11})$";
            var match = Regex.Match(request.ScanInput ?? "", pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var creationTime = DateTime.ParseExact(match.Groups["datetime"].Value, "yyMMdd-HHmmss", System.Globalization.CultureInfo.InvariantCulture);
                var serialCode = match.Groups["serial"].Value;

                var id = await _units.GetUnitIDBySerialCodeAsync(serialCode).ConfigureAwait(false);
                if (!id.HasValue)
                {
                    unitID = await _units.AddUnitAsync(request.LineCode, 0, serialCode, creationTime).ConfigureAwait(false);
                }
                else unitID = id.Value;
            }
            else
            {
                if (!_labelParser.TryParseNewWBFormat(request.ScanInput ?? "", out var label) || label == null)
                {
                    return new FailureUnpackUnitResponse($"Ocurrió un problema al procesar la entrada \"{request.ScanInput}\"");
                }

                unitID = label.UnitID;
            }

            return new SuccessUnpackUnitResponse(request.LineCode, unitID);
        }
    }
}