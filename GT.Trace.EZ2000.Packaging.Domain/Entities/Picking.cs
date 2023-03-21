namespace GT.Trace.EZ2000.Packaging.Domain.Entities
{
    public class Picking
    {
        /// <summary>
        /// Constructor que recibe todos estos parametros.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="period"></param>
        /// <param name="counter"></param>
        /// <param name="sequence"></param>
        /// <param name="totalSamples"></param>
        public Picking(ID id, int period, int counter, int sequence, int totalSamples)
        {
            ID = id;
            Period = period;
            Counter = counter;
            Sequence = sequence;
            TotalSamples = totalSamples;
        }

        /// <summary>
        /// Metodo que actualiza el contador de empaque, en caso de ser uno nuevo inicia todo en 0
        /// </summary>
        /// <param name="period"></param>
        public Picking(int period)
            : this(ID.New(), period, 0, 0, 0)
        {
            UpdateCounter();
        }

        /// <summary>
        /// Propiedad del constructro
        /// </summary>
        public int TotalSamples { get; }

        /// <summary>
        /// Propiedad del constructro
        /// </summary>
        public ID ID { get; }
        /// <summary>
        /// Propiedad del constructro
        /// </summary>
        public int Counter { get; private set; }
        /// <summary>
        /// Propiedad del constructro
        /// </summary>
        public int Sequence { get; private set; }
        /// <summary>
        /// Propiedad del constructro
        /// </summary>
        public int Period { get; }

        /// <summary>
        /// Propiedad boleana si esta activo o no 
        /// </summary>
        public bool IsActive => Counter >= Period;

        /// <summary>
        /// Pendiente por ver.
        /// </summary>
        private readonly HashSet<long> _units = new();
        /// <summary>
        /// Agregar Unidad donde recibe la unidad y la envia a _units
        /// </summary>
        /// <param name="id"></param>
        public void AddUnit(long id) => _units.Add(id);

        //metodo obtener unidades 
        public IReadOnlyCollection<long> GetUnits() => _units;

        /// <summary>
        /// Metodo actualizar, ver donde se utiliza
        /// </summary>
        public void Update()
        {
            if (!IsActive)
            {
                ++Counter;
            }
            else if (Sequence < TotalSamples)
            {
                Sequence++;
            }
            else
            {
                Counter = 0;
                Sequence = 0;
            }
        }
        /// <summary>
        /// Metodo actualizar conador
        /// Se utiliza en el metodo Picking
        /// </summary>
        public void UpdateCounter()
        {
            if (!IsActive)
            {
                ++Counter;
            }
        }
        /// <summary>
        /// Metodo actualizar Sequencia
        /// ver donde se utiliza
        /// </summary>
        public void UpdateSequence()
        {
            if (IsActive)
            {
                if (Sequence < TotalSamples - 1)
                {
                    Sequence++;
                }
                else
                {
                    Counter = 0;
                    Sequence = 0;
                }
            }
        }
    }
}
