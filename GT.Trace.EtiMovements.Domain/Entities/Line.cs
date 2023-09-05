using GT.Trace.Common;
using GT.Trace.EtiMovements.Domain.Events;

namespace GT.Trace.EtiMovements.Domain.Entities
{
    public sealed class Line
    {
        private readonly Queue<object> _events = new();

        public Line(string code, WorkOrder workOrder, IEnumerable<PointOfUse> bom)
        {
            Code = code;
            WorkOrder = workOrder;
            PointsOfUse = bom.ToList();
        }

        public string Code { get; }

        public WorkOrder WorkOrder { get; }

        public IReadOnlyList<PointOfUse> PointsOfUse { get; }

        public bool CanLoadEti(string pointOfUseCode, Eti eti, bool ignoreCapacity, out ErrorList errors)
        {
            errors = new();

            if (string.IsNullOrWhiteSpace(pointOfUseCode))
            {
                errors.Add("El código de túnel es requerido pero se encuentra en blanco.");
            }
            else if (!PointsOfUse.Any(item => item.Code == pointOfUseCode))
            {
                errors.Add($"El túnel {pointOfUseCode} no pertenece a la gama {Code} del modelo {WorkOrder.PartNo} Rev. {WorkOrder.Revision}.");
            }

            var componentEntries = PointsOfUse.Where(item => item.ComponentNo == eti.ComponentNo);
            if (!componentEntries.Any())
            {
                errors.Add($"El componente {eti.ComponentNo} no pertenece a la gama {Code} del modelo {WorkOrder.PartNo} Rev. {WorkOrder.Revision}.");
            }
            else
            {
                var pointOfUseEntry = componentEntries.SingleOrDefault(item => item.Code == pointOfUseCode);
                if (pointOfUseEntry == null)
                {
                    errors.Add($"El componente {eti.ComponentNo} no corresponde con el túnel {pointOfUseCode} en la gama {Code} del modelo {WorkOrder.PartNo} Rev. {WorkOrder.Revision}.");
                }
                else if (pointOfUseEntry.Revision.Number != eti.Revision.Number)
                {
                    errors.Add($"El componente {eti.ComponentNo} Rev. {eti.Revision.Number} no coincide con el del túnel {pointOfUseCode} del BOM {WorkOrder.PartNo} de la gama {Code}, {pointOfUseEntry.ComponentNo} Rev. {pointOfUseEntry.Revision.Number}.");
                }
                else if (!ignoreCapacity && !pointOfUseEntry.IsPartiallyLoadedOrEmpty)
                {
                    errors.Add($"El túnel {pointOfUseEntry.Code} se encuentra lleno con {pointOfUseEntry.LoadedEtis.Count} cajas del componente {eti.ComponentNo}.");
                }
                if (pointOfUseEntry != null && !pointOfUseEntry.CanLoad(eti, ignoreCapacity, out var errors2))
                {
                    errors.AddRange(errors2);
                }
            }

            return errors.IsEmpty;
        }

        public void LoadEti(string pointOfUseCode, Eti eti, bool ignoreCapacity)
        {
            if (!CanLoadEti(pointOfUseCode, eti, ignoreCapacity, out var errors)) throw errors.AsException();

            var pointOfUse = PointsOfUse.SingleOrDefault(item => item.ComponentNo == eti.ComponentNo && item.Code == pointOfUseCode);
            pointOfUse!.Load(eti, ignoreCapacity);
            _events.Enqueue(new EtiLoadedEvent(pointOfUse.Code, eti.Number, eti.ComponentNo, eti.LotNo, eti.LastMovement!.StartTime!.Value));
        }

        public bool CanUnloadEti(Eti eti, out ErrorList errors)
        {
            errors = new();
            return true;
            // this was implemented way before deciding the model was not that important
            //var pointOfUse = PointsOfUse.SingleOrDefault(item => item.ComponentNo == eti.ComponentNo && item.Code == eti.LastMovement!.PointOfUseCode);
            //if (pointOfUse != null)
            //{
            //    pointOfUse.CanUnload(eti, out errors);
            //}
            //else
            //{
            //    errors = new()
            //    {
            //        $"No se encontró el túnel {eti.LastMovement!.PointOfUseCode} desgnado para el componente {eti.ComponentNo} en el modelo {WorkOrder.PartNo} {Code}."
            //    };
            //}
            //return errors.IsEmpty;
        }

        public void UnloadEti(Eti eti)
        {
            if (!CanUnloadEti(eti, out var errors)) throw errors.AsException();

            var pointOfUse = PointsOfUse.SingleOrDefault(item => item.ComponentNo == eti.ComponentNo && item.Code == eti.LastMovement!.PointOfUseCode);
            if (pointOfUse != null)
            {
                pointOfUse!.Unload(eti);
            }
            _events.Enqueue(new EtiUnloadedEvent(eti.Number, DateTime.Now));
        }

        public bool CanUseEti(Eti eti, out ErrorList errors)
        {
            var pointOfUse = PointsOfUse.SingleOrDefault(item => item.ComponentNo == eti.ComponentNo && item.Code == eti.LastMovement!.PointOfUseCode);
            if (pointOfUse != null)
            {
                pointOfUse.CanUse(eti, out errors);
            }
            else
            {
                errors = new();
                errors.Add($"No se encontró el túnel {eti.LastMovement!.PointOfUseCode} desgnado para el componente {eti.ComponentNo} en el modelo {WorkOrder.PartNo} {Code}.");
            }
            return errors.IsEmpty;
        }

        public void UseEti(Eti eti)
        {
            if (!CanUseEti(eti, out var errors)) throw errors.AsException();

            var pointOfUse = PointsOfUse.SingleOrDefault(item => item.ComponentNo == eti.ComponentNo && item.Code == eti.LastMovement!.PointOfUseCode);
            var etiToReplace = pointOfUse!.ActiveEtiNo;
            pointOfUse!.Use(eti);
            _events.Enqueue(new EtiUsedEvent(eti.Number, etiToReplace, eti.LastMovement!.UsageTime!.Value));
        }

        public bool CanReturnEti(Eti eti, bool isChangeOver, out ErrorList errors)
        {
            var pointOfUse = PointsOfUse.SingleOrDefault(item => item.ComponentNo == eti.ComponentNo && item.Code == eti.LastMovement!.PointOfUseCode);
            errors = new();
            if (pointOfUse != null)
            {
                pointOfUse.CanReturn(eti, isChangeOver, out errors);
            }
            else
            {
                errors.Add($"No se encontró el túnel {eti.LastMovement!.PointOfUseCode} desgnado para el componente {eti.ComponentNo} en el modelo {WorkOrder.PartNo} {Code}.");
            }
            return errors.IsEmpty;
        }

        public void ReturnEti(Eti eti, bool isChangeOver)
        {
            if (!CanReturnEti(eti, isChangeOver, out var errors)) throw errors.AsException();

            var pointOfUse = PointsOfUse.SingleOrDefault(item => item.ComponentNo == eti.ComponentNo && item.Code == eti.LastMovement!.PointOfUseCode);
            pointOfUse!.Return(eti, isChangeOver);
            _events.Enqueue(new EtiReturnedEvent(eti.Number, eti.LastMovement!.EndTime!.Value));
        }

        public IReadOnlyList<object> GetEvents() => _events.ToList();
    }
}