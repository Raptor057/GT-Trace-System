using GT.Trace.Changeover.App.Gateways;
using GT.Trace.Common.CleanArch;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Serilog;
using System.ComponentModel;
using System.Reflection;

namespace GT.Trace.Changeover.App.UseCases.ApplyChangeover
{
    /// <summary>
    ///Este es el código de la clase ApplyChangeoverHandler, que es un componente del caso de uso ApplyChangeover. Esta clase implementa la interfaz IInteractor y es responsable de manejar el flujo de trabajo del caso de uso ApplyChangeover.
    ///La clase tiene una serie de dependencias que se inyectan a través del constructor, incluyendo varios gateways(interfaces que permiten acceder a datos específicos) y un servicio de impresión de etiquetas de retorno.Estas dependencias se utilizan dentro del método Handle para realizar las operaciones necesarias para realizar el cambio de modelo en una línea de producción.
    ///El método Handle toma una solicitud de cambio de modelo (ApplyChangeoverRequest) y devuelve una respuesta (ApplyChangeoverResponse). Primero se recupera la información de la línea de producción a través del LineGateway. Si la línea no existe, se devuelve una respuesta LineNotFoundResponse.A continuación, se recupera la información de la orden de trabajo actual de la línea a través del WorkOrderGateway.Si no hay una orden de trabajo en curso, se devuelve una respuesta WorkOrderNotFoundResponse.
    ///Luego, se verifica si un cambio de modelo es necesario comparando el código de orden de trabajo y el número de pieza actual de la línea con los de la orden de trabajo recuperada.Si no se necesita un cambio de modelo, se devuelve una respuesta ChangeoverNotRequiredResponse.
    ///Si se necesita un cambio de modelo, se recuperan los componentes salientes a través del GammaGateway y las Etiquetas de trazabilidad de los puntos de uso relacionados con esos componentes a través del PointOfUseGateway.Luego, se ejecuta el servicio de impresión de etiquetas de retorno para imprimir las etiquetas necesarias.Se actualiza la orden de trabajo a través del LineGateway y el cronograma de producción a través del ProductionScheduleGateway.Finalmente, se devuelve una respuesta ApplyChangeoverSuccessResponse que incluye cualquier error de impresión de etiquetas de retorno.
    /// </summary>
    internal sealed class ApplyChangeoverHandler : IInteractor<ApplyChangeoverRequest, ApplyChangeoverResponse>
    {
        private readonly ILogger<ApplyChangeoverHandler> _logger;
        private readonly ILineGateway _lines;
        private readonly IWorkOrderGateway _workOrders;
        private readonly IGammaGateway _gamma;
        private readonly IReturnLabelPrintingService _returnLabels;
        private readonly IProductionScheduleGateway _productionSchedule;
        private readonly IPointOfUseGateway _pointsOfUse;

        public ApplyChangeoverHandler(ILogger<ApplyChangeoverHandler> logger, ILineGateway lines, IWorkOrderGateway workOrders, IGammaGateway gamma, IReturnLabelPrintingService returnLabels, IProductionScheduleGateway productionSchedule, IPointOfUseGateway pointsOfUse)
        {
            _logger = logger;
            _lines = lines;
            _workOrders = workOrders;
            _gamma = gamma;
            _returnLabels = returnLabels;
            _productionSchedule = productionSchedule;
            _pointsOfUse = pointsOfUse;
        }

        public async Task<ApplyChangeoverResponse> Handle(ApplyChangeoverRequest request, CancellationToken cancellationToken)
        {
            var line = await _lines.GetLineAsync(request.LineCode).ConfigureAwait(false);
            if (line == null)
            {
                return new LineNotFoundResponse(request.LineCode);
            }

            _logger.LogInformation("{Line}", line);

            var workOrder = await _workOrders.GetLineWorkOrderAsync(line.ID).ConfigureAwait(false);
            if (workOrder == null)
            {
                return new WorkOrderNotFoundResponse(line.ID);
            }

            _logger.LogInformation("{WorkOrder}", workOrder);

            var changeoverIsRequired = !(line.WorkOrderCode == workOrder.Code && line.PartNo == workOrder.PartNo);
            if (!changeoverIsRequired)
            {
                return new ChangeoverNotRequiredResponse(line.Code);
            }

            //Agregado para corregir el BUG que no se actualiza la tabla LineProductionSchedule al aplicar cambio de modelo en cualquier linea
            var CountFindLineModelCapabilitiesAsync = await _productionSchedule.FindLineModelCapabilitiesAsync(request.LineCode, workOrder.PartNo).ConfigureAwait(false);
            if (!CountFindLineModelCapabilitiesAsync)
            {
                return new GammaNotFoundResponse($"!!!CAMBIO DE MODELO FALLIDO!!! no se encontro compatibilidad en la tabla [LineModelCapabilities] en GTT para el modelo: {workOrder.PartNo} para la linea: {request.LineCode} comunicate con el supervisor de linea / ingeniero de mejora continua / ingeniero programador");
            }

            if (line.Code != "LN")
            {
                //Se agrego para evitar el cambio de linea si falta la gamma en la base de datos
                //RA: 07/05/2023.
                var gammaData = await _gamma.GammaDataAsync(workOrder.PartNo, line.Code).ConfigureAwait(false);

                if (!gammaData)
                {
                    await _gamma.UpdateGamaTrazabAsync(workOrder.PartNo, line.Code).ConfigureAwait(false);
                    return new GammaNotFoundResponse($"!!!CAMBIO DE MODELO FALLIDO!!! no se encontro Gama de {workOrder.PartNo} para la linea {line.Code} actualiza esta ventana e intenta nueva mente, si no funciona comunicate con el supervisor de linea o con el ingeniero de mejora continua");
                }
                _logger.LogInformation("{GammaData}", gammaData);
            }
            ////Se agrego para evitar el cambio de linea si falta la gamma en la base de datos
            ////RA: 07/05/2023.
            //var gammaData = await _gamma.GammaDataAsync(workOrder.PartNo, line.Code).ConfigureAwait(false);
            //if(!gammaData)
            //{
            //    await _gamma.UpdateGamaTrazabAsync(workOrder.PartNo, line.Code).ConfigureAwait(false);
            //    return new GammaNotFoundResponse($"!!!CAMBIO DE MODELO FALLIDO!!! no se encontro Gama de {workOrder.PartNo} para la linea {line.Code} actualiza esta ventana e intenta nueva mente, si no funciona comunicate con el supervisor de linea o con el ingeniero de mejora continua");
            //}
            //_logger.LogInformation("{GammaData}", gammaData);

            var outgoingComponents = await _gamma.GetOutgoingComponentsAsync(line.PartNo, line.Code, workOrder.PartNo, line.Code).ConfigureAwait(false);

            _logger.LogInformation("{OutgoingComponents}", outgoingComponents);

            List<EtiDto> outgoingEtis = new();

            //using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            foreach (var item in outgoingComponents)
            {
                outgoingEtis.AddRange(
                    await _pointsOfUse.GetLoadedComponentEtis(item.CompNo, item.PointOfUseCode).ConfigureAwait(false)
                );
                //! Make sure the API call below updates the records.
                //await con.ExecuteAsync("UPDATE dbo.PointOfUseEtis SET UtcExpirationTime=GETUTCDATE(), Comments = 'CHANGEOVER' WHERE UtcExpirationTime IS NULL AND ComponentNo = @CompNo AND PointOfUseCode = @PointOfUse;", (object)item).ConfigureAwait(false);
            }
            //tx.Complete();

            _logger.LogInformation("{OutgoingEtis}", outgoingEtis);

            var errors = await _returnLabels.ExecuteAsync(request.LineCode, outgoingEtis.Select(item => item.Number).ToArray());

            _logger.LogInformation("{PrintErrors}", errors);

            await _lines.UpdateWorkOrderAsync(workOrder).ConfigureAwait(false);
            await _productionSchedule.UpdateProductionSchedule(request.LineCode, workOrder.PartNo, workOrder.Revision, workOrder.Code);

            return new ApplyChangeoverSuccessResponse(errors ?? new List<string>());
        }
    }
}