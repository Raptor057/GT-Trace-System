using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.Services;
using GT.Trace.Packaging.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.ComponentModel.Design;
using System.Text.RegularExpressions;

namespace GT.Trace.Packaging.App.UseCases.JoinFramelessMotors
{
    internal sealed class JoinFramelessMotorsHandler : IInteractor<JoinFramelessMotorsRequest, JoinFramelessMotorsResponse>
    {
        private readonly ILogger<JoinFramelessMotorsHandler> _logger;
        private readonly ILabelParserService _labelParser;
        private readonly IJoinFramelessMotorsGateway _gateway;

        public JoinFramelessMotorsHandler(ILogger<JoinFramelessMotorsHandler> logger, IJoinFramelessMotorsGateway gateway, ILabelParserService labelParser)
        {
            _gateway=gateway;
            _logger=logger;
            _labelParser=labelParser;

        }

        public async Task<JoinFramelessMotorsResponse> Handle(JoinFramelessMotorsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_labelParser.TryParseNewWBFormat(request.ScannerInputUnitID ?? "", out var label) && label != null)
                {
                    long unitID = label.UnitID;

                    if (_labelParser.TryParseFramelessFormat(request.ScannerInputComponentID ?? "", out var framelessMotorQR) && framelessMotorQR != null)
                    {
                        string QR = framelessMotorQR.SerialNumber;

                        if (request.LineCode == null || request.PartNo == null || request.LineCode == "" || request.PartNo == "")
                        {
                            var RegisteredInformation = await _gateway.FramelessRegisteredInformationAsync(unitID, QR) > 0;
                            if (RegisteredInformation)
                            {
                                await _gateway.DelJoinFramelessMotorsAsync(unitID, QR);
                                return new JoinFramelessMotorsSuccess($"Transmision {unitID} con Motor {QR} fueron eliminadas");
                            }
                            else
                            {
                                return new JoinFramelessMotorsFailure($"No se encontro informacion de la transmision {unitID} con Motor {QR}");
                            }
                        }
                        else
                        {
                            var RegisteredInformation = await _gateway.FramelessRegisteredInformationAsync(unitID, QR) > 0;
                            //var RegisteredInformationUnitID = await _gateway.FramelessRegisteredInformationUnitIDAsync(unitID);
                            //var RegisteredInformationComponentID = await _gateway.FramelessRegisteredInformationComponentIDAsync(QR);
                            if (!RegisteredInformation)
                            {
                                await _gateway.AddJoinFramelessMotorsAsync(unitID, QR, request.LineCode ?? "", request.PartNo);
                                return new JoinFramelessMotorsSuccess($"Transmision {unitID} enlazada con Motor {QR}");

                            }
                            else
                            {
                                return new JoinFramelessMotorsFailure($"La transmision {unitID} o el Motor {QR} ya han sido registrados con anterioridad");
                            }
                        }
                    }
                    else
                    {
                        return new JoinFramelessMotorsFailure("Etiqueta de Motor invalida");
                    }
                }
                else
                {
                    return new JoinFramelessMotorsFailure("Etiqueta individual invalida");
                }
            }
            catch (Exception ex)
            {
                return new JoinFramelessMotorsFailure(ex.Message);
            }
        }

    }
}
