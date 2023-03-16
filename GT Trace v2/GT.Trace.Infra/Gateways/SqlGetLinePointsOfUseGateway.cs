using GT.Trace.App.UseCases.Lines.GetPointsOfUse;
using GT.Trace.Infra.Daos;

namespace GT.Trace.Infra.Gateways
{
    internal class SqlGetLinePointsOfUseGateway : IGetPointsOfUseGateway
    {
        private readonly LinePointsOfUseDao _linePointsOfUse;

        public SqlGetLinePointsOfUseGateway(LinePointsOfUseDao linePointsOfUse)
        {
            _linePointsOfUse = linePointsOfUse;
        }

        public async Task<IEnumerable<EnabledPointOfUseDto>> GetEnabledPointsOfUseAsync(string lineCode)
        {
            var data = await _linePointsOfUse.FindLinePointsOfUseAsync(lineCode, false).ConfigureAwait(false);
            return data.Select(d => new EnabledPointOfUseDto(d.PointOfUseCode, d.CanBeLoadedByOperations)).ToArray();
        }
    }
}