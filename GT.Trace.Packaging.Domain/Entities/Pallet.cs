namespace GT.Trace.Packaging.Domain.Entities
{
    using Domain.Enums;
    using GT.Trace.Common;

    public sealed class Pallet
    {
        public static bool CanCreate(int size, int containerSize, out ErrorList errors)
        {
            errors = new();
            if (size < containerSize)
            {
                errors.Add($"La capacidad de la tarima [{size}] no puede ser menor que la capacidad del contenedor [{containerSize}].");
            }
            return errors.IsEmpty;
        }

        public static Pallet Create(int size, int containerSize, ContainerType containerType, Approval? approval, IReadOnlyCollection<Container> containers)
        {
            if (!CanCreate(size, containerSize, out var errors)) throw errors.AsException();
            return new Pallet(size, containerSize, containerType, approval, containers);
        }

        private readonly Queue<Container> _containers;

        private readonly int _containerSize;

        private readonly ContainerType _containerType;

        private Pallet(int size, int containerSize, ContainerType containerType, Approval? approval, IReadOnlyCollection<Container> containers)
        {
            Size = size;
            Approval = approval;
            _containerSize = containerSize;
            _containerType = containerType;
            _containers = new(containers);
            if (_containers.Count == 0) ChangeContainer();
        }

        public bool CanChangeContainer(out ErrorList errors)
        {
            errors = new();
            var container = GetActiveContainer();
            if (!(container?.IsFull ?? true))
            {
                errors.Add($"El contenedor #{container.ID} no se encuentra lleno.");
            }
            return errors.IsEmpty;
        }

        public void ChangeContainer()
        {
            if (!CanChangeContainer(out var errors)) throw errors.AsException();
            _containers.Enqueue(Container.Create(_containers.Count + 1, _containerSize, _containerType, new long[] { }));
        }

        public int Size { get; }

        public int Quantity => _containers.Sum(c => c.Quantity);

        public bool IsFull => Size <= Quantity;

        public bool IsPartial => Size > Quantity && Quantity > 0;

        public bool IsEmpty => Quantity == 0;

        public bool ContainerTypeIsBox => _containerType == ContainerType.Box;

        public bool HasQcApproval => Approval?.IsApproved ?? false;

        public Approval? Approval { get; private set; }

        public Container? GetActiveContainer() => _containers.LastOrDefault();

        public bool CanSetApproval(Approval approval, out ErrorList errors)
        {
            errors = new();
            if (approval.ID.Value <= 0)
            {
                errors.Add($"El número de aprobación no es válido [{approval.ID}].");
            }
            return errors.IsEmpty;
        }

        public void SetApproval(Approval approval)
        {
            if (!CanSetApproval(approval, out var errors)) throw errors.AsException();
            Approval = approval;
        }

        public bool CanPackUnit(long unitID, out ErrorList errors)
        {
            errors = new();
            if (IsFull)
            {
                errors.Add("La tarima se encuentra llena.");
            }
            if (!GetActiveContainer()!.CanPackUnit(unitID, out var errors2))
            {
                errors.Add(errors2.ToString());
            }
            var containerID = _containers.FirstOrDefault(c => c.Contains(unitID))?.ID;
            if (containerID.HasValue)
            {
                errors.Add($"Pieza {unitID} ya escaneada en contenedor #{containerID}.");
            }
            return errors.IsEmpty;
        }

        internal void PackUnit(long unitID)
        {
            if (!CanPackUnit(unitID, out var errors)) throw errors.AsException();
            GetActiveContainer()!.PackUnit(unitID);
        }

        public bool CanClear(out ErrorList errors)
        {
            errors = new();
            if (IsFull)
            {
                errors.Add("La tarima no se encuentra llena.");
            }
            return errors.IsEmpty;
        }

        public void Clear()
        {
            _containers.Clear();
            ChangeContainer();
        }

        public IReadOnlyCollection<long> GetUnits() => _containers.SelectMany(c => c.GetUnits()).ToList();
    }
}