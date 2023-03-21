namespace GT.Trace.EZ2000.Packaging.Domain.Entities
{
    using GT.Trace.Common;
    using GT.Trace.EZ2000.Packaging.Domain.Enums;

    public class Container
    {
        /// <summary>
        /// Puede crear contenedor, aqui se valida si se puede crear en contenedor, tiene los parametros de entrada 
        /// tamaño(size) y units (unidades), los cuales van a entrar y validar si el numero de unidades es menor que el tamaño del contenedor
        /// si es asi, dara una exepcion con el mensaje mostrado, si no regresara 0 errores 
        /// ErrorList es una clase en GT.Trace.Common.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="units"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static bool CanCreate(int size, IReadOnlyCollection<long> units, out ErrorList errors)
        {
            errors = new ErrorList();
            if (size < units.Count)
            {
                errors.Add($"El total de piezas [{units.Count}] no puede ser mayor que la capacidad [{size}].");
            }
            return errors.IsEmpty;
        }

        /// <summary>
        /// creacion de contenedor, aqui intenta crear el contenedor, ingresando el id de contenedor, tamaño, tipo, y cantidad de unidades.
        /// primero valida si si se puede crear el if con el !CanCreate significa si el metodo CanCreate retorno un true o un false, en caso 
        /// de haber retornado un false, arrojara el error que contiene el metodo !CanCreate
        /// en caso de que no retornara un nuevo contenedor con los datos que previamente lleva el contenedor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="size"></param>
        /// <param name="type"></param>
        /// <param name="units"></param>
        /// <returns></returns>
        public static Container Create(int id, int size, ContainerType type, IReadOnlyCollection<long> units)
        {
            if (!CanCreate(size, units, out var errors)) throw errors.AsException();
            return new Container(id, size, type, units);
        }
        private readonly Queue<long> _units;

        /// <summary>
        /// constructor de Contenedor, aqui se declara de lo que se compone el contenedor, cuando va a ser un contenedor nuevo creado.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="size"></param>
        /// <param name="type"></param>
        /// <param name="units"></param>
        public Container(int id, int size, ContainerType type, IReadOnlyCollection<long> units)
        {
            ID = id;
            Size = size;
            Type = type;
            _units = new(units);
        }
        /// <summary>
        /// estas 2 solo son propiedades
        /// </summary>
        public int ID { get; }

        public int Size { get; }

        /// <summary>
        /// el tipo de contenedor se establece en la clase ContainerType es un Enums(pendiente investigar eso)
        /// </summary>
        public ContainerType Type { get; }

        /// <summary>
        /// cantidad es igual al conteo de las unidades ingresadas al contenedor
        /// </summary>
        public int Quantity => _units.Count;

        /// <summary>
        /// esta propiedad establece si el contenedor esta full comparando la cantidad actual al momento del contenedor con el tamaño del contenedor
        /// </summary>
        public bool IsFull => Quantity >= Size;

        /// <summary>
        /// este metodo llamado Contains declara que recibe el unitID 
        /// </summary>
        /// <param name="unitID"></param>
        /// <returns></returns>
        public bool Contains(long unitID) => _units.Contains(unitID);

        /// <summary>
        /// se puede empacar unidad.
        /// aqui se ingresa el id de unidad,  en caso de que el contenedor este lleno dara un error,
        /// en caso de que la unidad ya se encuentre en contains, dira que ya fue escaneada.
        /// en caso de que no, este metodo regresa un errors.IsEmpty.
        /// </summary>
        /// <param name="unitID"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Metodo empacar unidad 
        /// recibe la unidad, y valida si se puede empacar haciendo uso del metodo !CanPackUnit
        /// si el resultado es false entonces empaca la unidad
        /// </summary>
        /// <param name="unitID"></param>
        public void PackUnit(long unitID)
        {
            if (!CanPackUnit(unitID, out var errors)) throw errors.AsException();
            _units.Enqueue(unitID);
        }
        /// <summary>
        /// este metodo aun esta pendiente
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<long> GetUnits() => _units;
    }
}
