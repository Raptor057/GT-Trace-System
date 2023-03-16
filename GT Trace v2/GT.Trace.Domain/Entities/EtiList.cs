namespace GT.Trace.Domain.Entities
{
    public sealed class EtiList
    {
        private readonly List<string> _items;

        public EtiList(string componentNo, int capacity, string? activeEtiNo, List<string>? loadedEtis)
        {
            ComponentNo = componentNo;
            Capacity = capacity;
            ActiveEtiNo = activeEtiNo;
            _items = new(capacity);
            if (loadedEtis != null) _items.AddRange(loadedEtis);
        }

        public string ComponentNo { get; }

        public int Capacity { get; }

        public string? ActiveEtiNo { get; private set; }

        public void Add(string etiNo) => _items.Add(etiNo);

        public void Use(string etiNo)
        {
            _items.Remove(etiNo);
            ActiveEtiNo = etiNo;
        }

        public void Remove(string etiNo) => _items.Remove(etiNo);

        public bool Contains(string etiNo) => _items.Contains(etiNo);

        public int Quantity => _items.Count;

        public IReadOnlyCollection<string> Items => _items;
    }
}