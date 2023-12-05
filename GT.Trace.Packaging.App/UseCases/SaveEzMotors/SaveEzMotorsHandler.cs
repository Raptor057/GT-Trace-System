using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.Services;
using Microsoft.Extensions.Logging;

namespace GT.Trace.Packaging.App.UseCases.SaveEzMotors
{
    internal sealed class SaveEzMotorsHandler:IInteractor<SaveEzMotorsRequest,SaveEzMotorsResponse>
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly ILogger<SaveEzMotorsHandler> _logger;
#pragma warning restore IDE0052 // Remove unread private members
        private readonly ISaveEzMotorsGateway _gateway;
        private readonly ILabelParserService _labelparser;

        public SaveEzMotorsHandler(ILogger<SaveEzMotorsHandler> logger, ISaveEzMotorsGateway gateway, ILabelParserService labelparser)
        {
            _logger = logger;
            _gateway = gateway;
            _labelparser = labelparser;
        }

        public async Task<SaveEzMotorsResponse> Handle(SaveEzMotorsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_labelparser.TryParseEZMotorsFormat(request.ScannerInputEzQR ?? "", out var label) && label != null)
                {
                    string model = request.Model;
                    string serialNumber = label.Motor_number;
                    string lineCode = request.LineCode;
                    bool GetEzMotors = await _gateway.GetEzMotorsDataAsync(model,serialNumber, lineCode).ConfigureAwait(false);

                    if (!GetEzMotors)
                    {
                        var pignon = await _gateway.GetPignonByPartNoAsync(model).ConfigureAwait(false);
                        var motor = await _gateway.GetMotorByPartNoAsync(model).ConfigureAwait (false);
                        await _gateway.AddEZMotorsDataAsync(model, serialNumber, lineCode,pignon,motor).ConfigureAwait(false);
                        return new SaveEzMotorsSuccess($"Datos del motor {serialNumber} registrados con exito del modelo {model} para la linea {lineCode}.");
                    }
                    else
                    {
                        return new SaveEzMotorsFailure($"Error el motor {serialNumber} ya ha sido registrado anteriormente en esta linea");
                        
                    }
                }
                else
                {
                    return new SaveEzMotorsFailure("No se pudo registrar el motor hay un problema con el QR o con el escaneo");
                }
            }
            catch (Exception ex)
            {
                return new SaveEzMotorsFailure(ex.Message);
            }
        }
    }
}
