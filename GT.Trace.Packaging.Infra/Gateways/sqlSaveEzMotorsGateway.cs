using GT.Trace.Packaging.App.UseCases.SaveEzMotors;
using GT.Trace.Packaging.Infra.DataSources;

namespace GT.Trace.Packaging.Infra.Gateways
{
    internal class SqlSaveEzMotorsGateway : ISaveEzMotorsGateway
    {
        private readonly GttSqlDB _gtt;
        //private readonly AppsSqlDB _apps;
        private readonly TrazaSqlDB _traza;

        public SqlSaveEzMotorsGateway(GttSqlDB gtt/*, AppsSqlDB apps*/, TrazaSqlDB traza)
        {
            _gtt=gtt;
            //_apps=apps;
            _traza=traza;
        }


        public async Task AddEZMotorsDataAsync(string model, string serialNumber, string Volt, string RPM, DateTime DateTimeMotor, string Rev, string lineCode, string pinionPartNum, string motorPartNum) =>
            await _gtt.AddEZMotorsData(model, serialNumber, Volt, RPM, DateTimeMotor, Rev, lineCode, pinionPartNum, motorPartNum);

        public async Task<bool> GetEzMotorsDataAsync(string model, string serialNumber, string lineCode, DateTime DateTimeMotor)
        {
            return await _gtt.GetEzMotorsData(model, serialNumber, lineCode, DateTimeMotor) > 0;
        }

        public async Task<string> GetMotorByPartNoAsync(string partno,string lineCode) =>
            await _traza.GetMotor(partno, lineCode);

        public async Task<string> GetPignonByPartNoAsync(string partno, string lineCode) =>
            await _traza.GetPignon(partno,lineCode);
    }
}
