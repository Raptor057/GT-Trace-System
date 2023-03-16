using GT.Trace.Changeover.App.UseCases.ApplyChangeover;
using GT.Trace.Changeover.Infra.Daos;

namespace GT.Trace.Changeover.Infra.Gateways
{
    internal class SqlPointOfUseGateway : IPointOfUseGateway
    {
        private readonly Daos.PointOfUseDao _pointsOfUse;

        public SqlPointOfUseGateway(PointOfUseDao pointsOfUse)
        {
            _pointsOfUse = pointsOfUse;
        }

        /// <summary>
        /// Return number of component ETIs loaded in the given point of use (including the active ones).
        /// </summary>
        /// <param name="componentNo">The component number.</param>
        /// <param name="pointOfUseCode">The point of use code.</param>
        /// <returns>An enumerable of EtiDto instances containing the ETI numbers.</returns>
        public async Task<IEnumerable<EtiDto>> GetLoadedComponentEtis(string componentNo, string pointOfUseCode)
        {
            var etis = await _pointsOfUse.GetLoadedEtis(componentNo, pointOfUseCode).ConfigureAwait(false);
            return etis.Select(eti => new EtiDto(eti.EtiNo));
        }
    }
}