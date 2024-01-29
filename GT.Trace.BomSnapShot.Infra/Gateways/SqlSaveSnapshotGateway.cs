
using Azure.Core;
using GT.Trace.BomSnapShot.App.UseCases.SaveSnapshot;
using GT.Trace.BomSnapShot.Infra.DataSources;
using GT.Trace.BomSnapShot.Infra.DataSources.Entities;

namespace GT.Trace.BomSnapShot.Infra.Gateways
{
    internal class SqlSaveSnapshotGateway : ISaveSnapshotGateway
    {
        private readonly GttSqlDB _gtt;
        private readonly AppsSqlDB _apps;
        private readonly TrazaSqlDB _traza;

        public SqlSaveSnapshotGateway(TrazaSqlDB traza, AppsSqlDB apps, GttSqlDB gtt)
        {
            _gtt=gtt;
            _apps=apps;
            _traza=traza;
        }

        //Esto guarda el snapshot
        public async Task<string> SaveSnapshotAsync(string pointOfUseCode, string componentNo)
        {
            try
            {
                var GetLinesCodesSharingPointOfUse = await _gtt.LinesCodesSharingPointOfUseAsync(pointOfUseCode, componentNo).ConfigureAwait(false);//Obtiene la lista de las lineas que usan ese tunel
                var LinesCodesSharingPointOfUseArray = GetLinesCodesSharingPointOfUse.ToArray();  // Convierte a un array
                bool CheckStatusActivecomponents = await _gtt.CheckStatusActivecomponentsAsync(pointOfUseCode).ConfigureAwait(false) > 0; //Valida si todos los componentes estan activos para poder realizar el snapshot

                if (LinesCodesSharingPointOfUseArray.Length <= 1)
                {
                    if (CheckStatusActivecomponents)
                    {
                        await _gtt.SaveSnapShotAsync(pointOfUseCode).ConfigureAwait(false);
                        return ($"Save SnapShot {LinesCodesSharingPointOfUseArray[0]} OK");
                    }
                    else
                    {
                        //return ($"Save SnapShot Fail [Gama de la linea [{LinesCodesSharingPointOfUseArray[1]}] incompleta]");
                        throw new InvalidOperationException($"Save SnapShot Fail [Gama de la linea [{LinesCodesSharingPointOfUseArray[0]}] incompleta]");
                    }
                }
                else if(LinesCodesSharingPointOfUseArray.Length > 1)
                {
                    List<string> lineCodes;
                    lineCodes = new List<string>();
                    for (int i = 0; i < LinesCodesSharingPointOfUseArray.Length; i++)
                    {
                        var pointOfUseCodeByLineCode = _gtt.GetPointofusecodebyLineCode(LinesCodesSharingPointOfUseArray[i]);
                        bool CheckStatusActivecomponentsSharing = await _gtt.CheckStatusActivecomponentsAsync(pointOfUseCodeByLineCode.Result.ToString()).ConfigureAwait(false) > 0; //Valida si todos los componentes estan activos para poder realizar el snapshot
                        if (CheckStatusActivecomponentsSharing)
                        {
                            await _gtt.SaveSnapShotAsync(pointOfUseCodeByLineCode.Result.ToString()).ConfigureAwait(false);
                            lineCodes.Add($"{LinesCodesSharingPointOfUseArray[i]} OK");
                        }
                        else
                        {
                            lineCodes.Add($"{LinesCodesSharingPointOfUseArray[i]} Fail");
                        }
                    }
                    string allLineCodes = string.Join(", ", lineCodes);
                    return $"Save SnapShot lines {allLineCodes}";
                }
                else
                {
                    throw new InvalidOperationException("Save SnapShot Fail");
                }

            }
            catch (Exception ex)
            {
                return (ex.ToString());
            }
}
    }
}
