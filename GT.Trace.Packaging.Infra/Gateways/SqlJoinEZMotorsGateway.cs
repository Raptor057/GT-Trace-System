﻿using GT.Trace.Packaging.App.UseCases.JoinEZMotors;
using GT.Trace.Packaging.Infra.DataSources;

namespace GT.Trace.Packaging.Infra.Gateways
{
    internal class SqlJoinEZMotorsGateway:IJoinEZMotorsGateway
    {
        private readonly GttSqlDB _gtt;

        public SqlJoinEZMotorsGateway(GttSqlDB gtt)
        {
            _gtt=gtt;
        }

        public async Task AddJoinEZMotorsAsync(long unitID, string Web1, string Current1, string Speed1, string Date1, string Time1, string Motor_Number1, string Web2, string Current2, string Speed2, string Date2, string Time2, string Motor_Number2)=>
            await _gtt.AddEZJoinMotors(unitID, Web1, Current1, Speed1, Date1, Time1, Motor_Number1, Web2, Current2, Speed2, Date2, Time2, Motor_Number2).ConfigureAwait(false);

        public async Task DelJoinEZMotorsAsync(long unitID)=>
            await _gtt.DelJoinEZMotors(unitID).ConfigureAwait(false);

        public async Task<int> EZRegisteredInformationAsync(long unitID, string Date1, string Time1, string Motor_Number1, string Date2, string Time2, string Motor_Number2)=>
            await _gtt.EZRegisteredInformation(unitID,Date1,Time1,Motor_Number1,Date2,Time2,Motor_Number2);

        //public Task<int> EZRegisteredInformationComponentIDAsync(string componentID)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<int> EZRegisteredInformationUnitIDAsync(long unitID)
        //{
        //    throw new NotImplementedException();
        //}
    }
}