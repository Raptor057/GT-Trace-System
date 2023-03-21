namespace GT.Trace.Packaging.Domain.Entities
{
    public class Picking
    {
        public const int DefaultTestUnits = 2;

        public Picking(ID id, int period, int counter, int sequence, int totalSamples)
        {
            ID = id;
            Period = period;
            Counter = counter;
            Sequence = sequence;
            TotalSamples = totalSamples;
        }

        public Picking(int period)
            : this(ID.New(), period, 0, 0, DefaultTestUnits)
        {
            UpdateCounter();
        }

        public int TotalSamples { get; }

        public ID ID { get; }

        public int Counter { get; private set; }

        public int Sequence { get; private set; }

        public int Period { get; }

        public bool IsActive => Counter >= Period;

        private readonly HashSet<long> _units = new();

        public void AddUnit(long id) => _units.Add(id);

        public IReadOnlyCollection<long> GetUnits() => _units;

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

        internal void UpdateCounter()
        {
            if (!IsActive)
            {
                ++Counter;
            }
        }

        internal void UpdateSequence()
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