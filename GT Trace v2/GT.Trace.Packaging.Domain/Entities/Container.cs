namespace GT.Trace.Packaging.Domain.Entities
{
    using Domain.Enums;
    using GT.Trace.Common;

    public class Container
    {
        public static bool CanCreate(int size, IReadOnlyCollection<long> units, out ErrorList errors)
        {
            errors = new ErrorList();
            if (size < units.Count)
            {
                errors.Add($"El total de piezas [{units.Count}] no puede ser mayor que la capacidad [{size}].");
            }
            return errors.IsEmpty;
        }

        public static Container Create(int id, int size, ContainerType type, IReadOnlyCollection<long> units)
        {
            if (!CanCreate(size, units, out var errors)) throw errors.AsException();
            return new Container(id, size, type, units);
        }

        private readonly Queue<long> _units;

        private Container(int id, int size, ContainerType type, IReadOnlyCollection<long> units)
        {
            ID = id;
            Size = size;
            Type = type;
            _units = new(units);
        }

        public int ID { get; }

        public int Size { get; }

        public ContainerType Type { get; }

        public int Quantity => _units.Count;

        public bool IsFull => Quantity >= Size;

        public bool Contains(long unitID) => _units.Contains(unitID);

        public bool CanPackUnit(long unitID, out ErrorList errors)
        {
            errors = new();
            if (IsFull)
            {
                errors.Add("El contenedor se encuentra lleno.");
            }
            if (Contains(unitID))
            {
                errors.Add($"Pieza {unitID} ya escaneada.");
            }
            return errors.IsEmpty;
        }

        public void PackUnit(long unitID)
        {
            if (!CanPackUnit(unitID, out var errors)) throw errors.AsException();
            _units.Enqueue(unitID);
        }

        public IReadOnlyCollection<long> GetUnits() => _units;
    }
}