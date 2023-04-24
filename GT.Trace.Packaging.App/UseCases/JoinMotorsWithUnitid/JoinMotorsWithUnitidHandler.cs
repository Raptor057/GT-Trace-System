using GT.Trace.Packaging.App.Services;
using GT.Trace.Packaging.App.UseCases.JoinMotorsWithUnitid.Responses;
using GT.Trace.Packaging.Domain.Repositories;
using System.Text.RegularExpressions;

namespace GT.Trace.Packaging.App.UseCases.JoinMotorsWithUnitid
{
    internal sealed class JoinMotorsWithUnitidHandler
    {
        private readonly ILabelParserService _labelParserService;
        private readonly IStationRepository _stations;
        private readonly IUnitRepository _units;

        public JoinMotorsWithUnitidHandler (ILabelParserService labelParserService, IStationRepository stations, IUnitRepository units)
        {
            _labelParserService=labelParserService;
            _stations=stations;
            _units=units;
        }

        public async Task<JoinMotorsWithUnitidResponse> Handle (JoinMotorsWithUnitidRequest request,CancellationToken cancellationToken)
        {
            long unidID;
            const string pattern = @"^.+\|.+\|(?<datetime>.+)\|(?<serial>.{11})$";
            var match = Regex.Match(request.ScannerImput ?? "", pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (match.Success)
            {
            

            }
            //var serialCode = match.Groups["serial"].Value;

            return new JoinMotorsWithUnitidResponse();
        }
    }
}
