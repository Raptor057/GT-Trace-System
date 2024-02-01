
using Azure.Core;
using GT.Trace.BomSnapShot.App.UseCases.SaveSnapshot;
using GT.Trace.BomSnapShot.Infra.DataSources;
using GT.Trace.BomSnapShot.Infra.DataSources.Entities;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

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
        public async Task<string> SaveSnapshotAsync(string etiNo, string lineCode)
        {

            try
            {

                #region Esto deberia de ser una clase aparte en otro file pero ahorita no hay tiempo para eso
                //RA: 02/01/2024
                // Expresión regular
                string pattern = @"^(\d+)?(?<no>([sS][aA](?<id>\d{1,}))|([eE][sS](?<id>\d{1,}))|([eE](?<id>\d{1,})(-[tT]\d{1,})?)).?$";
                string _Etino;
                // Comparar etiNo con la expresión regular
                Match match = Regex.Match(etiNo, pattern);

                // Verificar si hay una coincidencia
                if (match.Success)
                {
                    // Obtener el valor del grupo 'no'
                   _Etino = match.Groups["no"].Value;
                }
                else
                {
                    // No hubo coincidencia con la expresión regular
                    return  "No hay coincidencia con la expresión regular";
                }
                #endregion
                PointOfUseEtis? etiNoInfo;
                etiNoInfo = await _gtt.GetEtiLastMovementAsync(_Etino.Trim()).ConfigureAwait(false);
                if (etiNoInfo == null) return $"Eti {etiNo.Trim()} no encontrada";

                pro_production? pro_Prod;
                pro_Prod = await _apps.GetLineProductionActiveByPointOfUseCodeAsync(lineCode).ConfigureAwait(false);
                if(pro_Prod == null) return $"Orden activa no encontrada";
                var partNo = pro_Prod.part_number.Trim();

                int ActiveBomComponentCountByLineCode = await _gtt.ActiveBomComponentCountByLineCodeAsync(lineCode).ConfigureAwait(false); //Valor de los componentes activos
                int BomComponentCountByLineCodeAndPartNo = await _traza.BomComponentCountByLineCodeAndPartNoAsync(lineCode,partNo).ConfigureAwait(false); //Valor de los componentes del boom
                var GetlinesCodesSharingPointOfUse = await _apps.GetLinesCodesSharingPointOfUseAsync(etiNoInfo.PointOfUseCode, etiNoInfo.ComponentNo).ConfigureAwait(false);
                var LinesCodesSharingPointOfUseArray = GetlinesCodesSharingPointOfUse.Select(line => new string[] { line.LineCode ?? "", line.Model ?? ""}).ToArray();

                var CheckStatusActivecomponents = ActiveBomComponentCountByLineCode == BomComponentCountByLineCodeAndPartNo || ActiveBomComponentCountByLineCode - 1 == BomComponentCountByLineCodeAndPartNo - 1;


                if (LinesCodesSharingPointOfUseArray.Length <= 1)
                {
                    if (CheckStatusActivecomponents)
                    {
                        await _gtt.SaveSnapShotAsync(etiNoInfo.PointOfUseCode).ConfigureAwait(false);
                        var row = LinesCodesSharingPointOfUseArray[0];
                        return ($"Save SnapShot Linea: {row[0]} Modelo: {row[1]} OK");
                    }
                    else
                    {
                        return ($"Save SnapShot Fail [Gama de la linea {lineCode} incompleta]");
                        //throw new InvalidOperationException($"Save SnapShot Fail [Gama de la linea incompleta]");
                    }
                }
                else if(LinesCodesSharingPointOfUseArray.Length > 1)
                {
                    List<string> lineCodes;
                    lineCodes = new List<string>();

                    // Recorriendo filas
                    for (int i = 0; i < LinesCodesSharingPointOfUseArray.Length; i++)
                    {
                        // Acceder al array interno (columnas) en la fila i
                        var row = LinesCodesSharingPointOfUseArray[i];
                        var pointOfUseCodeByLineCode = _gtt.GetPointofusecodebyLineCodeAsync(row[0]);
                        int ActiveBomComponentCountByLineCodes = await _gtt.ActiveBomComponentCountByLineCodeAsync(row[0]).ConfigureAwait(false); //Valor de los componentes activos
                        int BomComponentSCountByLineCodeAndPartNo = await _traza.BomComponentCountByLineCodeAndPartNoAsync(row[0], row[1]).ConfigureAwait(false); //Valor de los componentes del boom
                        var CheckStatusActivecomponentsSharing = ActiveBomComponentCountByLineCodes == BomComponentSCountByLineCodeAndPartNo || ActiveBomComponentCountByLineCodes - 1 == BomComponentSCountByLineCodeAndPartNo - 1;

                        if (CheckStatusActivecomponentsSharing)
                        {
                            await _gtt.SaveSnapShotAsync(pointOfUseCodeByLineCode.Result.ToString()).ConfigureAwait(false);
                            lineCodes.Add($"Linea: {row[0]} Modelo: {row[1]} OK");
                        }
                        else
                        {
                            lineCodes.Add($"Linea: {row[0]} Modelo: {row[1]} Fail");
                        }

                    }
                    string allLineCodes = string.Join(", ", lineCodes);
                    return $"Save SnapShot lines {allLineCodes}";
                }
                else
                {
                    return $"Save SnapShot {lineCode} Fail";
                    //throw new InvalidOperationException("Save SnapShot Fail");
                }

            }
            catch (Exception ex)
            {
                return (ex.ToString());
            }
}
    }
}
