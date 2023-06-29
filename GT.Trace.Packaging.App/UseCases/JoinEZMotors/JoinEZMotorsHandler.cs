using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.Services;
using Microsoft.Extensions.Logging;

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
                if(_labelParser.TryParseNewWBFormat(request.ScannerInputUnitID ?? "", out var label) && label != null)
                {
                    long unitID = label.UnitID;
                        if (_labelParser.TryParseEZMotorsFormat(request.ScannerInputMotorID1 ?? "", out var labeldata)&& labeldata != null)
                        {
                            string QRMotor1 = labeldata.Motor_number;

                            if (_labelParser.TryParseEZMotorsFormat(request.ScannerInputMotorID2 ?? "", out var labeldata2) && labeldata2 != null)
                            {

                                string QRMotor2 = labeldata2.Motor_number;
                                if (Equals(QRMotor1,QRMotor2))
                                {
                                    return new JoinEZMotorsFailure($"Las Unidades Motor 1:[{QRMotor1}], Motor 2:[{QRMotor2}] son iguales");
                                }
                                    if (request.IsEnable == 0)
                                    {
                                        await _gateway.DelJoinEZMotorsAsync(unitID).ConfigureAwait(false);
                                        return new JoinEZMotorsSuccess($"Transmision {label.UnitID} desenlazada de {labeldata.Motor_number} & {labeldata2.Motor_number}");
                                    }
                                    else
                                    {
                                        var RegisteredInformation = await _gateway.EZRegisteredInformationAsync(label.UnitID, labeldata.Date, labeldata.Time, labeldata.Motor_number, labeldata2.Date, labeldata2.Time, labeldata2.Motor_number) > 0;

                                        if (!RegisteredInformation)
                                        {
                                            await _gateway.AddJoinEZMotorsAsync(unitID, labeldata.Website, labeldata.No_Load_Current, labeldata.No_Load_Speed, labeldata.Date, labeldata.Time, labeldata.Motor_number, labeldata2.Website, labeldata2.No_Load_Current, labeldata2.No_Load_Speed, labeldata2.Date, labeldata2.Time, labeldata2.Motor_number);
                                            return new JoinEZMotorsSuccess($"Transmision {label.UnitID} enlazada con {labeldata.Motor_number} & {labeldata2.Motor_number}");
                                        }
                                        else
                                        {
                                            return new JoinEZMotorsFailure($"Las Unidades {label.UnitID},{labeldata.Motor_number},{labeldata2.Motor_number} ya cuenta con un registro");
                                        }
                                    }
                            }
                            else
                            {
                                return new JoinEZMotorsFailure($"Eti {request.ScannerInputMotorID2} no es valida");
                            }
                        }
                        else
                        {
                            return new JoinEZMotorsFailure($"Eti {request.ScannerInputMotorID1} no es valida");
                        }
                }
                else
                {
                    return new JoinEZMotorsFailure($"Eti {request.ScannerInputUnitID} no es valida");
                }
            }
            catch (Exception ex)
            {
                return new JoinEZMotorsFailure(ex.Message);
            }
        }
    }
}
