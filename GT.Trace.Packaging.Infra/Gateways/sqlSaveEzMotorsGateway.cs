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


        public async Task AddEZMotorsDataAsync(string model, string serialNumber, string lineCode, string pinionPartNum, string motorPartNum) =>
            await _gtt.AddEZMotorsData(model, serialNumber, lineCode,pinionPartNum,motorPartNum);

        public async Task<bool> GetEzMotorsDataAsync(string model, string serialNumber, string lineCode)
        {
            return await _gtt.GetEzMotorsData(model, serialNumber, lineCode) > 0;
        }

        public async Task<string> GetMotorByPartNoAsync(string partno)=>
            await _traza.GetMotor(partno);

        public async Task<string> GetPignonByPartNoAsync(string partno) =>
            await _traza.GetPignon(partno);
    }
}
