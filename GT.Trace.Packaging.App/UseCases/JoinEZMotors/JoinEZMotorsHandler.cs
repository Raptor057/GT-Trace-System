using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.Services;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace GT.Trace.Packaging.App.UseCases.JoinEZMotors
{
    internal sealed class JoinEZMotorsHandler : IInteractor<JoinEZMotorsRequest, JoinEZMotorsResponse>
    {
        private readonly ILogger<JoinEZMotorsHandler> _logger;
        private readonly IJoinEZMotorsGateway _gateway;
        private readonly ILabelParserService _labelParser;

        public JoinEZMotorsHandler(ILogger<JoinEZMotorsHandler> logger, IJoinEZMotorsGateway gateway, ILabelParserService labelParser)
        {
            _logger=logger;
            _gateway=gateway;
            _labelParser=labelParser;

        }

        public async Task<JoinEZMotorsResponse> Handle(JoinEZMotorsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                #region Old
                //if(_labelParser.TryParseNewWBFormat(request.ScannerInputUnitID ?? "", out var label) && label != null)
                //{
                //    long unitID = label.UnitID;
                //        if (_labelParser.TryParseEZMotorsFormat(request.ScannerInputMotorID1 ?? "", out var labeldata)&& labeldata != null)
                //        {
                //            string QRMotor1 = labeldata.Motor_number;

                //            if (_labelParser.TryParseEZMotorsFormat(request.ScannerInputMotorID2 ?? "", out var labeldata2) && labeldata2 != null)
                //            {

                //                string QRMotor2 = labeldata2.Motor_number;
                //                if (Equals(QRMotor1,QRMotor2))
                //                {
                //                    return new JoinEZMotorsFailure($"Las Unidades Motor 1:[{QRMotor1}], Motor 2:[{QRMotor2}] son iguales");
                //                }
                //                    if (request.IsEnable == 0)
                //                    {
                //                        await _gateway.DelJoinEZMotorsAsync(unitID).ConfigureAwait(false);
                //                        return new JoinEZMotorsSuccess($"Transmision {label.UnitID} desenlazada de {labeldata.Motor_number} & {labeldata2.Motor_number}");
                //                    }
                //                    else
                //                    {
                //                        DateTime MotorDateTime1 = DateTime.ParseExact($"{labeldata.Date} {labeldata.Time}", "yyyy-M-d HH:mm", CultureInfo.InvariantCulture);
                //                        DateTime MotorDateTime2 = DateTime.ParseExact($"{labeldata2.Date} {labeldata2.Time}", "yyyy-M-d HH:mm", CultureInfo.InvariantCulture);

                //                        var RegisteredInformation = await _gateway.EZRegisteredInformationAsync(label.UnitID, labeldata.Date, labeldata.Time, labeldata.Motor_number, labeldata2.Date, labeldata2.Time, labeldata2.Motor_number) > 0;
                //                        var MotorsData1 = await _gateway.EZRegisteredInformationAsync(labeldata.Motor_number,MotorDateTime1).ConfigureAwait(false)>0;
                //                        var MotorsData2 = await _gateway.EZRegisteredInformationAsync(labeldata2.Motor_number, MotorDateTime2).ConfigureAwait(false)>0;

                //                        if (!MotorsData1)
                //                        {
                //                         return new JoinEZMotorsFailure($"La Unidad {labeldata.Motor_number} no cuenta con un registro de proceso previo");
                //                        }

                //                        if (!MotorsData2)
                //                        {
                //                         return new JoinEZMotorsFailure($"La Unidad {labeldata2.Motor_number} no cuenta con un registro de proceso previo");
                //                        }

                //                        if (!RegisteredInformation)
                //                        {
                //                            await _gateway.AddJoinEZMotorsAsync(unitID, labeldata.Website, labeldata.No_Load_Current, labeldata.No_Load_Speed, labeldata.Date, labeldata.Time, labeldata.Motor_number,labeldata.PN,labeldata.AEM,labeldata.Rev, labeldata2.Website, labeldata2.No_Load_Current, labeldata2.No_Load_Speed, labeldata2.Date, labeldata2.Time, labeldata2.Motor_number, labeldata2.PN,labeldata2.AEM,labeldata2.Rev);
                //                            return new JoinEZMotorsSuccess($"Transmision {label.UnitID} enlazada con {labeldata.Motor_number} & {labeldata2.Motor_number}");
                //                        }
                //                        else
                //                        {
                //                            return new JoinEZMotorsFailure($"Las Unidades {label.UnitID},{labeldata.Motor_number},{labeldata2.Motor_number} ya cuenta con un registro");
                //                        }
                //                    }
                //            }
                //            else
                //            {
                //                return new JoinEZMotorsFailure($"Eti {request.ScannerInputMotorID2} no es valida");
                //            }
                //        }
                //        else
                //        {
                //            return new JoinEZMotorsFailure($"Eti {request.ScannerInputMotorID1} no es valida");
                //        }
                //}
                //else
                //{
                //    return new JoinEZMotorsFailure($"Eti {request.ScannerInputUnitID} no es valida");
                //}
                //--------------------------
                #endregion
                #region "Mejorado"
                if (!_labelParser.TryParseNewWBFormat(request.ScannerInputUnitID ?? "", out var label) && label != null)
                {
                    return new JoinEZMotorsFailure($"Eti {request.ScannerInputUnitID} no es valida");
                }
                long unitID = label.UnitID;

                if (!_labelParser.TryParseEZMotorsFormat(request.ScannerInputMotorID1 ?? "", out var labeldata) && labeldata != null)
                {
                    return new JoinEZMotorsFailure($"Eti {request.ScannerInputMotorID1} no es valida");
                }
                string QRMotor1 = labeldata.Motor_number;

                if (!_labelParser.TryParseEZMotorsFormat(request.ScannerInputMotorID2 ?? "", out var labeldata2) && labeldata2 != null)
                {
                    return new JoinEZMotorsFailure($"Eti {request.ScannerInputMotorID1} no es valida");
                }
                string QRMotor2 = labeldata2.Motor_number;

                if (Equals(QRMotor1, QRMotor2))
                {
                    return new JoinEZMotorsFailure($"Las Unidades Motor 1:[{QRMotor1}], Motor 2:[{QRMotor2}] son iguales");
                }

                if (request.IsEnable == 0)
                {
                    await _gateway.DelJoinEZMotorsAsync(unitID).ConfigureAwait(false);
                    return new JoinEZMotorsSuccess($"Transmision {label.UnitID} desenlazada de {labeldata.Motor_number} & {labeldata2.Motor_number}");
                }

                DateTime MotorDateTime1 = DateTime.ParseExact($"{labeldata.Date} {labeldata.Time}", "yyyy-M-d HH:mm", CultureInfo.InvariantCulture);
                DateTime MotorDateTime2 = DateTime.ParseExact($"{labeldata2.Date} {labeldata2.Time}", "yyyy-M-d HH:mm", CultureInfo.InvariantCulture);
                var RegisteredInformation = await _gateway.EZRegisteredInformationAsync(label.UnitID, labeldata.Date, labeldata.Time, labeldata.Motor_number, labeldata2.Date, labeldata2.Time, labeldata2.Motor_number) > 0;
                var ActiveEZModel = await _gateway.GetEZModelAsync().ConfigureAwait(false);

                if (RegisteredInformation)
                {
                    return new JoinEZMotorsFailure($"Las Unidades {label.UnitID},{labeldata.Motor_number},{labeldata2.Motor_number} ya cuenta con un registro en esta estacion");
                }

                if (ActiveEZModel != "87254") //Esto fue agregado para el EZ22000
                {
                var MotorsData1 = await _gateway.EZRegisteredInformationAsync(labeldata.Motor_number, MotorDateTime1).ConfigureAwait(false) > 0;
                var MotorsData2 = await _gateway.EZRegisteredInformationAsync(labeldata2.Motor_number, MotorDateTime2).ConfigureAwait(false) > 0;

                if (!MotorsData1)
                {
                    return new JoinEZMotorsFailure($"La Unidad {labeldata.Motor_number} no cuenta con un registro de proceso previo en la tabla [gtt].[dbo].[MotorsData]");
                }

                if (!MotorsData2)
                {
                    return new JoinEZMotorsFailure($"La Unidad {labeldata2.Motor_number} no cuenta con un registro de proceso previo en la tabla [gtt].[dbo].[MotorsData]");
                }
                    var Motors1Pinions = await _gateway.EZMotorDataRegisteredInformationAsync(labeldata.Motor_number, MotorDateTime1).ConfigureAwait(false) > 0;
                    var Motors2Pinions = await _gateway.EZMotorDataRegisteredInformationAsync(labeldata2.Motor_number, MotorDateTime2).ConfigureAwait(false) > 0;
                    if (!Motors1Pinions)
                    {
                        return new JoinEZMotorsFailure($"El Subensamble escaneado no es compatible con el modelo que se esta corriendo en esta linea");
                    }
                    if (!Motors2Pinions)
                    {
                        return new JoinEZMotorsFailure($"El Subensamble escaneado no es compatible con el modelo que se esta corriendo en esta linea");
                    }
                }
                await _gateway.AddJoinEZMotorsAsync(unitID, labeldata.Website, labeldata.No_Load_Current, labeldata.No_Load_Speed, labeldata.Date, labeldata.Time, labeldata.Motor_number, labeldata.PN, labeldata.AEM, labeldata.Rev);
                await _gateway.AddJoinEZMotorsAsync(unitID,labeldata2.Website, labeldata2.No_Load_Current, labeldata2.No_Load_Speed, labeldata2.Date, labeldata2.Time, labeldata2.Motor_number, labeldata2.PN, labeldata2.AEM, labeldata2.Rev);
                return new JoinEZMotorsSuccess($"Transmision {label.UnitID} enlazada con {labeldata.Motor_number} & {labeldata2.Motor_number}");
                #endregion
            }
            catch (Exception ex)
            {
                return new JoinEZMotorsFailure(ex.Message);
            }
        }
    }
}
