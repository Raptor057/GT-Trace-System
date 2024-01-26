using GT.Trace.TraceabilityLegacyIntegration.Infra.DataSources;

namespace GT.Trace.TraceabilityLegacyIntegration.Infra.Gateways
{
    internal class SqlMasterCancellation
    {
        private readonly TrazaSqlDB _Traza;

        public SqlMasterCancellation(TrazaSqlDB Traza)
        {
            _Traza = Traza;
        }


    }
}
