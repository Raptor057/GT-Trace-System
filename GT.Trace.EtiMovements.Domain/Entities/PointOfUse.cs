using GT.Trace.Common;

namespace GT.Trace.EtiMovements.Domain.Entities
{
    public sealed class PointOfUse
    {
        public PointOfUse(string code, string componentNo, Revision revision, int capacity, IEnumerable<string> loadedEtis, string? activeEtiNo)
        {
            Code = code;
            ComponentNo = componentNo;
            Capacity = capacity;
            Revision = revision;
            LoadedEtis = loadedEtis.ToList();
            ActiveEtiNo = activeEtiNo;
        }

        public string Code { get; }

        public string ComponentNo { get; }

        public Revision Revision { get; set; }

        public int Capacity { get; set; }

        public IReadOnlyList<string> LoadedEtis { get; set; }

        public string? ActiveEtiNo { get; set; }

        public bool IsPartiallyLoadedOrEmpty => LoadedEtis.Count < Capacity;

        public bool CanLoad(Eti eti, bool ignoreCapacity, out ErrorList errors)
        {
            eti.CanLoad(Code, out errors);
            if (LoadedEtis.Contains(eti.Number))
            {
                errors.Add($"ETI {eti.Number} ya se encuentra cargada en el túnel {Code}.");
            }
            if (string.Compare(ActiveEtiNo, eti.Number, true) == 0)
            {
                errors.Add($"ETI {eti.Number} ya se encuentra en uso en el túnel {Code}.");
            }
            if (!ignoreCapacity && Capacity <= LoadedEtis.Count)
            {
                errors.Add($"La capacidad [ {Capacity} ] del túnel {Code} se encuentra al máximo [ {LoadedEtis.Count} ].");
            }
            return errors.IsEmpty;
        }

        public void Load(Eti eti, bool ignoreCapacity)
        {
            if (!CanLoad(eti, ignoreCapacity, out var errors)) throw errors.AsException();
            eti.Load(Code);
        }

        public bool CanUnload(Eti eti, out ErrorList errors)
        {
            eti.CanUnload(out errors);
            if (!LoadedEtis.Contains(eti.Number))
            {
                errors.Add($"ETI {eti.Number} no se encuentra cargada en el túnel {Code}.");
            }
            return errors.IsEmpty;
        }

        public void Unload(Eti eti)
        {
            if (!CanUnload(eti, out var errors)) throw errors.AsException();
            eti.Unload();
        }

        public bool CanUse(Eti eti, out ErrorList errors)
        {
            eti.CanUse(out errors);
            if (!LoadedEtis.Contains(eti.Number))
            {
                errors.Add($"ETI {eti.Number} no se encuentra cargada en el túnel {Code}.");
            }
            return errors.IsEmpty;
        }

        public void Use(Eti eti)
        {
            if (!CanUse(eti, out var errors)) throw errors.AsException();
            eti.Use();
            ActiveEtiNo = eti.Number;
        }

        public bool CanReturn(Eti eti, bool isChangeOver, out ErrorList errors)
        {
            eti.CanReturn(isChangeOver, out errors);

            // We want to also return ETIs not in use.

            //if (string.IsNullOrWhiteSpace(ActiveEtiNo))
            //{
            //    errors.Add($"No hay una ETI activa actualmente en el túnel {Code}.");
            //}
            //else if (string.Compare(eti.Number, ActiveEtiNo, true) != 0)
            //{
            //    errors.Add($"No se puede retornar la ETI {eti.Number}. La ETI activa en el túnel {Code} es {ActiveEtiNo}.");
            //}
            return errors.IsEmpty;
        }

        public void Return(Eti eti, bool isChangeOver)
        {
            if (!CanReturn(eti, isChangeOver, out var errors)) throw errors.AsException();
            eti.Return(isChangeOver);
            ActiveEtiNo = null;
        }
    }
}