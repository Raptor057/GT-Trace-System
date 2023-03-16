using GT.Trace.Common;
using GT.Trace.Domain.Events;

namespace GT.Trace.Domain.Entities
{
    public sealed class Line
    {
        private readonly Queue<object> _events = new();

        private readonly Dictionary<string, PointOfUse> _pointsOfUse = new();

        public Line(int id, string code, string name, WorkOrder workOrder, Part? activePart, IEnumerable<BomComponent> bom, IEnumerable<SetComponent> set, Dictionary<string, Dictionary<string, List<string>>>? loadedEtisByPointOfUse, bool outputIsSubAssembly)
        {
            Id = id;
            Code = code;
            Name = name;
            WorkOrder = workOrder;
            ActivePart = activePart;
            OutputIsSubAssembly = outputIsSubAssembly;
            foreach (var pointOfUseCode in bom.Select(item => item.PointOfUseCode).Distinct())
            {
                _pointsOfUse.Add(
                    pointOfUseCode,
                    new PointOfUse(
                        pointOfUseCode,
                        bom.Where(item => item.PointOfUseCode == pointOfUseCode),
                        set.Where(item => item.PointOfUseCode == pointOfUseCode),
                        loadedEtisByPointOfUse?.ContainsKey(pointOfUseCode) ?? false ? loadedEtisByPointOfUse?[pointOfUseCode] : null));
            }
        }

        public bool OutputIsSubAssembly { get; }

        public int Id { get; }

        public string Code { get; }

        public string Name { get; }

        public WorkOrder WorkOrder { get; }

        public Part? ActivePart { get; }

        public IReadOnlyDictionary<string, PointOfUse> PointsOfUse => _pointsOfUse;

        public IReadOnlyCollection<object> GetEvents() => _events;

        public bool CanUseMaterial(string etiNo, out ErrorList errors)
        {
            errors = new();
            if (!_pointsOfUse.Any(pou => pou.Value.ContainsEti(etiNo)))
            {
                errors.Add($"ETI [ {etiNo} ] no se encuentra cargada en la línea [ {Code} ].");
            }
            return errors.IsEmpty;
        }

        public void UseMaterial(string etiNo)
        {
            if (!CanUseMaterial(etiNo, out var errors)) throw errors.AsException();
            _pointsOfUse.Single(pou => pou.Value.ContainsEti(etiNo)).Value.UseMaterial(etiNo);
        }

        public bool CanLoadMaterial(string operatorNo, Eti eti, string pointOfUseCode, out ErrorList errors)
        {
            errors = new();
            if (string.IsNullOrWhiteSpace(operatorNo))
            {
                errors.Add("El código de operador se encuentra en blanco.");
            }
            if (string.IsNullOrWhiteSpace(pointOfUseCode))
            {
                errors.Add("El código de túnel no puede estar en blanco.");
            }
            else if (!_pointsOfUse.ContainsKey(pointOfUseCode))
            {
                errors.Add($"El túnel \"{pointOfUseCode}\" no se encuentra configurado en la línea \"{Code}\".");
            }
            else if (!_pointsOfUse[pointOfUseCode].ComponentIsConfigured(eti.Component.Number))
            {
                errors.Add($"El componente \"{eti.Component.Number}\" no corresponde con el túnel \"{pointOfUseCode}\".");
            }
            else if (_pointsOfUse[pointOfUseCode].CheckIsFull(eti.Component.Number))
            {
                errors.Add($"El componente \"{eti.Component.Number}\" se encuentra a su capacidad máxima en el túnel \"{pointOfUseCode}\".");
            }
            if (eti.Id < 1)
            {
                errors.Add($"El identificador de ETI [ {eti.Id} ] no es válido. El identificador necesita ser un entero positivo.");
            }
            if (string.IsNullOrWhiteSpace(eti.Number))
            {
                errors.Add("El número de ETI se encuentra en blanco.");
            }
            if (!eti.IsEnabled)
            {
                errors.Add($"ETI#{eti.Number} se encuentra deshabilitada.");
            }
            if (eti.IsLoaded)
            {
                errors.Add($"ETI [ {eti.Number} ] se encuentra cargada desde [ {eti.Status?.EffectiveTime:dd-MMM-yyyy HH:mm} hrs. ] en el túnel [ {eti.Status?.PointOfUseCode} ].");
            }
            else if (eti.IsUsed)
            {
                errors.Add($"ETI [ {eti.Number} ] ya fue utilizada en [ {eti.Status?.UsageTime:dd-MMM-yyyy HH:mm} hrs. ] en el túnel [ {eti.Status?.PointOfUseCode} ].");
            }
            else if (eti.IsActive)
            {
                errors.Add($"ETI [ {eti.Number} ] ya está siendo utilizada desde [ {eti.Status?.UsageTime:dd-MMM-yyyy HH:mm} hrs. ] en el túnel [ {eti.Status?.PointOfUseCode} ].");
            }
            return errors.IsEmpty;
        }

        public void LoadMaterial(string operatorNo, Eti eti, string pointOfUseCode, string partNo)
        {
            if (!CanLoadMaterial(operatorNo, eti, pointOfUseCode, out var errors)) throw errors.AsException();
            _pointsOfUse[pointOfUseCode].LoadMaterial(eti);

            eti.SetLoadStatus(pointOfUseCode);

            _events.Enqueue(new MaterialLoadedEvent(operatorNo, pointOfUseCode, eti.Number, partNo, eti.LotNo, WorkOrder.Id, WorkOrder.Code, WorkOrder.Order, WorkOrder.LineOrder, eti.Component.Number, "CARGA NORMAL"));
        }

        public bool CanUnloadMaterial(Eti eti, out ErrorList errors)
        {
            //Console.WriteLine(eti);
            var pointOfUseCode = eti.Status?.PointOfUseCode ?? "";

            errors = new();
            if (eti.Status == null)
            {
                errors.Add($"ETI [ {eti.Number} ] no se encuentra cargada en ningún túnel.");
            }
            else if (!_pointsOfUse.ContainsKey(pointOfUseCode))
            {
                errors.Add($"El túnel [ {pointOfUseCode} ] en el que se encuentra la ETI [ {eti.Number} ] no pertenece a la línea [ {Code} ].");
            }
            else if (!_pointsOfUse[pointOfUseCode].ComponentIsConfigured(eti.Component.Number))
            {
                errors.Add($"El componente [ {eti.Component.Number} ] de la ETI [ {eti.Number} ] no se encuentra configurado en la línea [ {Code} ].");
            }
            else if (eti.Status?.UsageTime.HasValue ?? false)
            {
                errors.Add($"ETI [ {eti.Number} ] está siendo utilizada en el túnel [ {pointOfUseCode} ] desde [ {eti.Status?.UsageTime:dd-MMM-yyyy HH:mm} ].");
            }
            //else if (!_pointsOfUse[pointOfUseCode].ContainsEti(eti.Number))
            //{
            //    errors.Add($"ETI [ {eti.Number} ] no se encuentra cargada en la línea [ {Code} ].");
            //}
            return errors.IsEmpty;
        }

        public void UnloadMaterial(Eti eti)
        {
            if (!CanUnloadMaterial(eti, out var errors)) throw errors.AsException();
            var pointOfUseCode = eti.Status?.PointOfUseCode ?? throw new NullReferenceException("Código de túnel en blanco.");
            _pointsOfUse[pointOfUseCode].UnloadMaterial(eti);
            eti.ClearStatus();
            _events.Enqueue(new MaterialUnloadedEvent(Code, pointOfUseCode, eti.Number, WorkOrder.Part.Number, WorkOrder.Code));
        }

        public bool CanUseMaterial(Eti eti, out ErrorList errors)
        {
            var pointOfUseCode = eti.Status?.PointOfUseCode ?? "";
            errors = new();
            if (string.IsNullOrWhiteSpace(pointOfUseCode))
            {
                errors.Add($"ETI [ {eti.Number} ] no se encuentra cargada en ningún túnel.");
            }
            else if (!_pointsOfUse.ContainsKey(pointOfUseCode))
            {
                errors.Add($"El túnel [ {pointOfUseCode} ] en el que se encuentra la ETI [ {eti.Number} ] no pertenece a la línea [ {Code} ].");
            }
            else if (!_pointsOfUse[pointOfUseCode].ComponentIsConfigured(eti.Component.Number))
            {
                errors.Add($"El componente [ {eti.Component.Number} ] de la ETI [ {eti.Number} ] no se encuentra configurado en la línea [ {Code} ].");
            }
            else if (!_pointsOfUse[pointOfUseCode].ContainsEti(eti.Number))
            {
                errors.Add($"ETI [ {eti.Number} ] no se encuentra cargada en la línea [ {Code} ].");
            }
            if (eti.Status?.UsageTime.HasValue ?? false)
            {
                errors.Add($"ETI [ {eti.Number} ] ya ha sido utilizada en [ {eti.Status?.UsageTime:dd-MMM-yyyy HH:mm} ]");
            }
            return errors.IsEmpty;
        }

        public void UseMaterial(Eti eti)
        {
            if (!CanUseMaterial(eti, out var errors)) throw errors.AsException();
            var pointOfUseCode = eti.Status?.PointOfUseCode ?? throw new NullReferenceException("Código de túnel en blanco.");
            var activeEti = _pointsOfUse[pointOfUseCode].GetActiveEti(eti.Component.Number);
            _pointsOfUse[pointOfUseCode].UseMaterial(eti.Number);
            _events.Enqueue(new MaterialUsedEvent(Code, pointOfUseCode, eti.Number, WorkOrder.Part.Number, WorkOrder.Code));
            if (!string.IsNullOrWhiteSpace(activeEti))
            {
                _events.Enqueue(new MaterialReturnedEvent(Code, pointOfUseCode, activeEti, WorkOrder.Part.Number, WorkOrder.Code, true));
            }
        }

        public bool CanReturnMaterial(Eti eti, out ErrorList errors)
        {
            //Console.WriteLine(eti);
            var pointOfUseCode = eti.Status?.PointOfUseCode ?? "";
            errors = new();
            if (string.IsNullOrWhiteSpace(pointOfUseCode))
            {
                errors.Add($"ETI [ {eti.Number} ] no se encuentra cargada en ningún túnel.");
            }
            else if (!_pointsOfUse.ContainsKey(pointOfUseCode))
            {
                errors.Add($"El túnel [ {pointOfUseCode} ] en el que se encuentra la ETI [ {eti.Number} ] no pertenece a la línea [ {Code} ].");
            }
            else if (!_pointsOfUse[pointOfUseCode].ComponentIsConfigured(eti.Component.Number))
            {
                errors.Add($"El componente [ {eti.Component.Number} ] de la ETI [ {eti.Number} ] no se encuentra configurado en la línea [ {Code} ].");
            }
            else if (string.Compare(_pointsOfUse[pointOfUseCode].GetActiveEti(eti.Component.Number), eti.Number, true) != 0)
            {
                errors.Add($"ETI [ {eti.Number} ] no se encuentra activa.");
            }
            else if (!eti.IsActive)
            {
                errors.Add($"ETI [ {eti.Number} ] no esta siendo utilizada.");
            }
            return errors.IsEmpty;
        }

        public void ReturnMaterial(Eti eti)
        {
            if (!CanReturnMaterial(eti, out var errors)) throw errors.AsException();
            var pointOfUseCode = eti.Status?.PointOfUseCode ?? throw new NullReferenceException("Código de túnel en blanco.");
            _pointsOfUse[pointOfUseCode].UnloadMaterial(eti);
            _events.Enqueue(new MaterialReturnedEvent(Code, pointOfUseCode, eti.Number, WorkOrder.Part.Number, WorkOrder.Code, false));
        }
    }
}