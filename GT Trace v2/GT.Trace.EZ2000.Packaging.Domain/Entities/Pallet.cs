namespace GT.Trace.EZ2000.Packaging.Domain.Entities
{
    using GT.Trace.Common;
    using GT.Trace.EZ2000.Packaging.Domain.Enums;
    public sealed class Pallet
    {


        /// <summary>
        /// Este medoto regresa un valor voleano donde valida que la capacidad de la tarima no sea menor que la capacidad del contenedor
        /// si no cumple con esa condicion regresa un error, si cumple, entonces regresa un 0.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="containerSize"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static bool CanCreate(int size, int containerSize, out ErrorList errors)
        {
            errors = new();
            if (size > containerSize)
            {
                errors.Add($"La capacidad de la tarima [{size} no puede ser menor que la capacidad del contenedor [{containerSize}]]");
            }
            return errors.IsEmpty;
        }

        /// <summary>
        /// Crear Pallet, primero valida si se puede crear enviando los datos al metodo !CanCreate, si no regresa errores entonces
        /// retorna un nuevo Pallet con sus respectivos datos.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="containerSize"></param>
        /// <param name="containerType"></param>
        /// <param name="approval"></param>
        /// <param name="containers"></param>
        /// <returns></returns>
        public static Pallet Create(int size, int containerSize, ContainerType containerType, Approval? approval, IReadOnlyCollection<Container> containers)
        {
            if (!CanCreate(size, containerSize, out var errors)) throw errors.AsException();
            return new Pallet(size, containerSize, containerType, approval, containers);
        }
        /// <summary>
        /// la propiedad _containers viene del constructor container de la clase Container.
        /// el contructor contiene los parametros para generar un nuevo contenedor.
        /// </summary>
        private readonly Queue<Container> _containers;
        /// <summary>
        /// propiedad
        /// </summary>
        private readonly int _containerSize;
        /// <summary>
        /// tipo de contenedor que viene de la clase ContainerType que es un enum.
        /// </summary>
        private readonly ContainerType _containerType;

        /// <summary>
        /// Constructor Pallet, que especifica lo que necesita contener un Pallet para ser creado.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="containerSize"></param>
        /// <param name="containerType"></param>
        /// <param name="approval"></param>
        /// <param name="containers"></param>
        public Pallet(int size, int containerSize, ContainerType containerType, Approval? approval, IReadOnlyCollection<Container> containers)
        {
            Size = size;
            Approval = approval;
            _containerSize = containerSize;
            _containerType = containerType;
            _containers = new(containers);
            if (_containers.Count == 0) ChangeContainer(); //Esta condicion dicta que si el conteo de contenedores es 0 entonces ejecuta cambio de contenedor
        }

        /// <summary>
        /// Metodo boleano
        /// se puede cambiar contenedor
        /// obtiene el contenedor activo, luego valida si el contenedor esta lleno, si no esta lleno, si no regresa nada entonces es false, si
        /// esta lleno va a regresar un true y eso producira un error.
        /// si al final el contenedor esta lleno entonces retorna 0
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Metodo camcio de contenedor
        /// hace una validacion si el contenedor se puede cambiar, si el contenedor no esta lleno entonces,
        /// se le suma una pieza a contenedor.
        /// </summary>
        public void ChangeContainer()
        {
            if (!CanChangeContainer(out var errors)) throw errors.AsException();
            _containers.Enqueue(Container.Create(_containers.Count + 1, _containerSize, _containerType, new long[] { }));
        }
        /// <summary>
        /// Propiedad Tamaño usada en el contrusctor
        /// </summary>
        public int Size { get; }
        /// <summary>
        /// Propiedad cantidad donde suma todos las unidades de container para reflejar la cantidad.
        /// </summary>
        public int Quantity => _containers.Sum(c => c.Quantity);
        /// <summary>
        /// propiedad boleana para validar si esta lleno o no.
        /// </summary>
        public bool IsFull => Size <= Quantity;
        /// <summary>
        /// propiedad boleana para saber si es parcial.
        /// </summary>
        public bool IsPartial => Size > Quantity && Quantity > 0;
        /// <summary>
        /// propiedad boleana para validar si el contenedor esta vacio.
        /// </summary>
        public bool IsEmpty => Quantity == 0;
        /// <summary>
        /// propiedad boleana para validar si el tipo de contenedor es una caja
        /// </summary>
        public bool ContainerTypeIsBox => _containerType == ContainerType.Box;
        /// <summary>
        /// propiedad boleana para validar si esta aprovado por calidad.
        /// </summary>
        public bool HasQcApproval => Approval?.IsApproved ?? false;
        /// <summary>
        /// propiedad usada en el constructor
        /// </summary>
        public Approval? Approval { get; private set; }
        /// <summary>
        /// metodo para obtener el contenedor activo que va a la clase Container y hace uso del metodo para dar ese resultado , mediante el contrusctor.
        /// </summary>
        /// <returns></returns>
        public Container? GetActiveContainer() => _containers.LastOrDefault();
        /// <summary>
        /// metodo boleano para validar si Puede establecer la aprobación 
        /// </summary>
        /// <param name="approval"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public bool CanSetApproval(Approval approval, out ErrorList errors)
        {
            errors = new();
            if (approval.ID.Value <= 0)
            {
                errors.Add($"El numero de aprobacion no es valido [{approval.ID}].");
            }
            return errors.IsEmpty;
        }
        /// <summary>
        /// Metodo boleano para establecer la aprovacion, que para retronar el valor primero hace una validacion
        /// al metodo !CanSetApproval si este regresa un true entonces se establece la aporvacion en la variable approval del contrsuctor.
        /// </summary>
        /// <param name="approval"></param>
        public void SetApproval(Approval approval)
        {
            if (!CanSetApproval(approval, out var errors)) throw errors.AsException();
            Approval = approval;
        }
        /// <summary>
        /// Metodo boleano para validar si se puede empacar la unidad, hace 2 validaciones.
        /// si la tarima se encuentra llena, y si la pieza ya fue escaneada perviamente.
        /// </summary>
        /// <param name="unitID"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Metodo para empacar unidad, primero hace una validacion si se puede empacar la unidad en el metodo !CanPackUnit,
        /// si no regresa ningun error entonces ejecuta el metodo GetActiveContainer y luego el metodo PackUnit para empacar la unidad en el contenedor
        /// correspondiente
        /// </summary>
        /// <param name="unitID"></param>
        internal void PackUnit(long unitID)
        {
            if (!CanPackUnit(unitID, out var errors)) throw errors.AsException();
            GetActiveContainer()!.PackUnit(unitID);
        }
        /// <summary>
        /// se puede borrar aqui hace una validacion si se puede borrar "pendiente por ver"
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public bool CanClear(out ErrorList errors)
        {
            errors = new();
            if (IsFull)
            {
                errors.Add("La tarima no se encuentra llena.");
            }
            return errors.IsEmpty;
        }
        /// <summary>
        /// Limpiar, literalmente borra todo de _containers y cambia de contenedor
        /// </summary>
        public void Clear()
        {
            _containers.Clear();
            ChangeContainer();
        }
        /// <summary>
        /// Metodo que obtiene las unidades en forma de lista de _containers.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<long> GetUnits() => _containers.SelectMany(c => c.GetUnits()).ToList();
    }
}
