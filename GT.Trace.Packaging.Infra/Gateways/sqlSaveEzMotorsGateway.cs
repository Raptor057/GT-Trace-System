using GT.Trace.Packaging.App.UseCases.SaveEzMotors;
using GT.Trace.Packaging.Infra.DataSources;

namespace GT.Trace.Packaging.Infra.Gateways
{
    internal class SqlSaveEzMotorsGateway : ISaveEzMotorsGateway
    {
        private readonly GttSqlDB _gtt;

        public SqlSaveEzMotorsGateway(GttSqlDB gtt)
        {
            _gtt=gtt;
        }

        public async Task AddEZMotorsDataAsync(string model, string serialNumber, string lineCode)
        {
            await _gtt.AddEZMotorsData(model, serialNumber, lineCode);
        }

        public async Task<bool> GetEzMotorsDataAsync(string model, string serialNumber, string lineCode)
        {
            return await _gtt.GetEzMotorsData(model, serialNumber, lineCode) > 0;
        }
    }
}
