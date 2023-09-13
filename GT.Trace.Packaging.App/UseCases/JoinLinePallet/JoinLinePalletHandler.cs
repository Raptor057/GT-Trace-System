using GT.Trace.Common.CleanArch;
using GT.Trace.Packaging.App.Services;
using GT.Trace.Packaging.App.UseCases.JoinEZMotors;
using Microsoft.Extensions.Logging;

namespace GT.Trace.Packaging.App.UseCases.JoinLinePallet
{
    internal sealed class JoinLinePalletHandler : IInteractor<JoinLinePalletRequest, JoinLinePalletResponse>
    {
        private readonly ILogger<JoinLinePalletHandler> _logger;
        private readonly IJoinLinePalletGateway _gateway;
        private readonly ILabelParserService _labelParser;

        public JoinLinePalletHandler(ILogger<JoinLinePalletHandler> logger, IJoinLinePalletGateway gateway, ILabelParserService labelParser)
        {
            _logger=logger;
            _gateway=gateway;
            _labelParser=labelParser;

        }
        public async Task<JoinLinePalletResponse> Handle(JoinLinePalletRequest request, CancellationToken cancellationToken)
        {
            try
            {
                //throw new NotImplementedException();
                if(_labelParser.TryParseNewWBFormat(request.ScannerInputUnitID ?? "", out var labelData) && labelData != null)
                {
                    long unitID = labelData.UnitID;
                    
                    if(_labelParser.TryParsePalletFormat(request.ScannerInputPallet ?? "",out var palletData) && palletData != null)
                    {
                        string palletID = palletData.palletQR;

                        //if (request.IsEnable == 0)
                        //{
                        //    //throw new NotImplementedException(); //Para evitar el error.
                        //    return new JoinLinePalletSucess($"Etiqueta {unitID} desenlazada del Pallet {palletID}[ESTO AUN NO ESTA PROGRAMADO]");

                        //}
                        //else
                        //{
                        //    //var RegisteredInformation = await _gateway.PalletRegisteredInformationAsync(labelData.UnitID) > 0;
                        //    await _gateway.AddJoinPalletAsync(unitID, palletID,request.LineCode);
                        //    return new JoinLinePalletSucess($"Etiqueta {unitID} enlazada con Pallet {palletID}");
                        //}

                        //var RegisteredInformation = await _gateway.PalletRegisteredInformationAsync(labelData.UnitID) > 0;
                        await _gateway.AddJoinPalletAsync(unitID, palletID, request.LineCode);
                        return new JoinLinePalletSucess($"Etiqueta {unitID} enlazada con Pallet {palletID}");
                    }
                    else
                    {
                        //string palletID = palletData.palletQR;
                        return new JoinLinePalletFailure($"QR del Pallet {request.ScannerInputPallet} no es valida");
                    }
                }
                else
                {
                    return new JoinLinePalletFailure($"Eti {request.ScannerInputUnitID} no es valida");
                }
            }
            catch (Exception ex)
            {
                return new JoinLinePalletFailure(ex.Message);
            }
        }
    }
}
