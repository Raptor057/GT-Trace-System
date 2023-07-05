namespace GT.Trace.Packaging.Domain.Entities
{
    using Domain.Enums;
    using GT.Trace.Common;
    using GT.Trace.Packaging.Domain.Events;

    public record Station(
        string ProcessName,
        bool IsLocked,
        bool CanValidateRevision,
        bool CanValidateCustomerPartNo,
        bool CanValidateFunctionalTest,
        bool RequireTrace,
        bool CanTrace,
        bool CanPick,
        bool CanAutoload,
        bool CanChangeLine,
        Line Line)
    {
        public const string PackagingProcessName = "PACK";

        private readonly Queue<object> _events = new();

        public IReadOnlyCollection<object> GetEvents() => _events;

        public bool ValidateLabel(Label label, out ErrorList errors)
        {
            errors = new();
            //if (Line.POIsRequired)
            //{
            //    errors.Add("PO# no puede estar vacio cuando el empaque es tipo caja. Informe al supervisor.");
            //}

            var aux = Line.WorkOrder.Part.Number.Split("-");

            if (string.Compare(label.Part.Number, aux[0], true) != 0)
            {
                errors.Add($"Etiqueta [{label.Part.Number}] No Pertenece al Modelo o Numero Parte Actual en Producción [{aux[0]}].");
            }

            if (CanValidateCustomerPartNo && string.Compare(label.ClientPartNo, Line.WorkOrder.CustomerPartNo, true) != 0)
            {
                errors.Add($"NP Cliente en etiqueta [{label.ClientPartNo}] No Coincide con el Numero Parte Actual en Producción [{Line.WorkOrder.CustomerPartNo}].");
            }

            if (CanValidateRevision && string.Compare(Line.WorkOrder.Part.Revision.Number, label.Part.Revision.Number, true) != 0)
            {
                errors.Add($"Revision de TM [{label.Part.Revision}] No Coincide con la Rev. Actual en Producción [{Line.WorkOrder.Part.Revision}].");
            }

            return errors.IsEmpty;
        }

        public bool TryPickUnit(Unit unit)
        {
            var success = false;
            var picking = Line.Picking ?? throw new InvalidOperationException("La referencia de picking es nula.");
            if (CanPick && picking.IsActive)
            {
                Line.Picking.UpdateSequence();
                Line.Picking.AddUnit(unit.ID);
                _events.Enqueue(new UnitPickedEvent(unit.ID, Line.WorkOrder.Part.Number, picking.Sequence, Line.WorkOrder.Code, Line.Name));
                success = true;
            }
            else
            {
                picking.UpdateCounter();
            }
            _events.Enqueue(new PickingUpdatedEvent(picking.ID.Value, picking.IsActive, picking.Counter, picking.Sequence));
            return success;
        }

        public bool IsPackagingProcess => string.Compare(ProcessName, PackagingProcessName, true) == 0;

        public bool CanPackUnit(Unit unit, out ErrorList errors)
        {
            errors = new();
            if (Line.Headcount < 1)
            {
                errors.Add($"No se ha especificado la cantidad de operadores en la línea {Line.Code} para el cálculo del PPH.");
            }
            if (unit.PackagingInfo != null)
            {
                errors.Add($"La pieza #{unit.ID} ya se encuentra empacada en la línea {unit.PackagingInfo.LineName} desde {unit.PackagingInfo.TimeStamp:el \\día dd-MMM-yy a la\\s HH:mm \\hora\\s}.");
            }

            if (unit.MasterIds.Count > 0)
            {
                errors.Add($"La pieza #{unit.ID} ya se encuentra asociada a las masters {unit.MasterIds.Select(m => $"M{m}").Aggregate((x, y) => $"{x}, {y}")}.");
            }

            if (unit.PickingReference.HasValue)
            {
                errors.Add($"La pieza #{unit.ID} fue seleccionada para picking (Ref#{unit.PickingReference}).");
            }

            if (!Line.ValidPackagingSources.Contains(unit.CurrentProcessNo))
            {
                if (Line.ValidPackagingSources.Length > 0)
                    errors.Add($"La pieza proviene del proceso \"{unit.CurrentProcessNo}\" pero el proceso de empaque \"999\" requiere que la pieza provenga de: {Line.ValidPackagingSources.Aggregate((a, b) => $"{a}, {b}")}");
                else
                    errors.Add($"La pieza proviene del proceso \"{unit.CurrentProcessNo}\" y no es un proceso previo válido para empaque \"999\".");
            }

            if (CanValidateFunctionalTest)
            {
                if (!unit.IsTested)
                {
                    errors.Add($"Pieza #{unit.ID} no tiene prueba funcional.");
                }
                else if (!(unit.FunctionalTestStatus ?? false))
                {
                    errors.Add($"Pieza #{unit.ID} tiene prueba con estatus NOK.");
                }
            }
            if (RequireTrace && !unit.IsTraced)
            {
                errors.Add($"TM {unit.ID} No fue Trazada en Estacion Inicial!!");
            }
            if (!Line.Pallet.CanPackUnit(unit.ID, out var canPackUnitErrors))
            {
                errors.AddRange(canPackUnitErrors);
            }
            if (Line.QcContainerApprovalIsRequired)
            {
                errors.Add("Se Requiere Liberacion de Auditor de Calidad Para Seguir Empacando!!");
            }

            if (Line.Bom.Components.Any(com => string.IsNullOrWhiteSpace(com.EtiNo)))
            {
                errors.Add($"La trazabilidad de componentes en la línea \"{Line.Code}\" se encuentra incompleta.");
            }

            return errors.IsEmpty;
        }

        public void PackUnit(Unit unit)
        {
            if (!CanPackUnit(unit, out var errors)) throw errors.AsException();

            if (CanTrace)
            {
                unit.SetTrace(new Trace(Line.Name, DateTime.UtcNow));
                //Aqui se agrego como nuevo el Line.Code
                _events.Enqueue(new UnitTracedEvent(unit.ID, Line.WorkOrder.Part.Number, Line.WorkOrder.Code, Line.Name,Line.Code ,-1, Line.WorkOrder.Client.Name));
            }

            Line.Pallet.PackUnit(unit.ID);
            _events.Enqueue(new UnitPackedEvent(Line.ID, Line.Code, Line.Name, Line.WorkOrder.Code, Line.WorkOrder.Client.Name, Line.WorkOrder.Part.Number, Line.WorkOrder.Part.Revision.Number, unit.ID, "", false, null, null));
        }

        public bool TryCreateQcApprovalRecord()
        {
            // we're about to pack the first unit
            if (Line.Pallet.Quantity == 0 || Line.Pallet.Approval == null)
            {
                Line.Pallet.SetApproval(new Approval(false, null));
                _events.Enqueue(new ContainerApprovalCreatedEvent(Line.Pallet.Approval?.ID.Value ?? 0,
                    Line.Name, Line.WorkOrder.Part.Number, Line.WorkOrder.Code, Line.WorkOrder.CustomerPartNo, Line.WorkOrder.Part.Revision.Number, Line.Pallet.Size, Line.Pallet.GetActiveContainer()?.Size ?? 0));
                return true;
            }

            return false;
        }

        public bool TryResetJrContainerIfFull()
        {
            var container = Line.Pallet.GetActiveContainer();
            if (container!.IsFull)
            {
                Line.Pallet.ChangeContainer();
                var result = container.Type == ContainerType.Box;
                return result;
            }

            return false;
        }

        public bool TryResetMasterContainerIfFull()
        {
            if (Line.Pallet.IsFull)
            {
                var master = new MasterLabel(Line.Pallet.GetUnits());
                Line.Pallet.Approval?.SetMasterLabelReference(master.ID);

                _events.Enqueue(new PalletCompletedEvent(Line.WorkOrder.Client.Name, Line.WorkOrder.Part.Number, Line.WorkOrder.Part.Revision.Number, Line.WorkOrder.CustomerPartNo, Line.WorkOrder.Part.Description ?? "",
                    Line.WorkOrder.PO.Number, "-", Line.WorkOrder.Client.Description, Line.Name, true, Line.Pallet.Quantity, Line.WorkOrder.MasterType.ToString(), false, false, $"{DateTime.Now.DayOfYear:000}",
                    Line.WorkOrder.Part.ProductFamily ?? "", Line.WorkOrder.Code, Line.Pallet.Approval!.ID.Value, Line.Pallet.Approval!.Username, Line.Pallet.Approval!.Date));

                // NOTE: Update approval record.

                Line.Pallet.Clear();
                return true;
            }

            return false;
        }

        public bool CanResetPartialContainer(out ErrorList errors)
        {
            errors = new();
            if (Line.Pallet.IsEmpty && !Line.Pallet.IsPartial)
            {
                errors.Add("La tarima se encuentra vacía.");
            }
            if (!Line.Pallet.HasQcApproval)
            {
                errors.Add("La tarima no cuenta con la aprobación de calidad. Necesita ser liberada.");
            }
            return errors.IsEmpty;
        }

        public void ResetPartialContainer()
        {
            if (!CanResetPartialContainer(out var errors)) throw errors.AsException();

            var master = new MasterLabel(Line.Pallet.GetUnits());
            Line.Pallet.Approval?.SetMasterLabelReference(master.ID);

            _events.Enqueue(new PalletCompletedEvent(Line.WorkOrder.Client.Name, Line.WorkOrder.Part.Number, Line.WorkOrder.Part.Revision.Number, Line.WorkOrder.CustomerPartNo, Line.WorkOrder.Part.Description ?? "",
                Line.WorkOrder.PO.Number, "-", Line.WorkOrder.Client.Description, Line.Name, true, Line.Pallet.Quantity, Line.WorkOrder.MasterType.ToString(), false, true, $"{DateTime.Now.DayOfYear:000}",
                Line.WorkOrder.Part.ProductFamily ?? "", Line.WorkOrder.Code, Line.Pallet.Approval!.ID.Value, Line.Pallet.Approval!.Username!, Line.Pallet.Approval!.Date!.Value));

            // NOTE: Update approval record.

            Line.Pallet.Clear();
        }
    }
}