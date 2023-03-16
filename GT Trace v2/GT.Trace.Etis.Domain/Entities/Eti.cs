namespace GT.Trace.Etis.Domain.Entities
{
    public sealed class Eti
    {
        public static bool CheckEtiIsComponent(string etiNo) =>
            !string.IsNullOrWhiteSpace(etiNo) && etiNo.Length >= 1
            && etiNo[0] == '5';

        public static bool CheckEtiIsAssembly(string etiNo) =>
            !string.IsNullOrWhiteSpace(etiNo) && etiNo.Length >= 1
            && etiNo[0] == 'E';

        public static bool CheckEtiIsServicePart(string etiNo) =>
            !string.IsNullOrWhiteSpace(etiNo) && etiNo.Length >= 2
            && string.Compare(etiNo[..2], "GT", true) == 0;

        public static bool CheckEtiIsSubAssembly(string etiNo) =>
            !string.IsNullOrWhiteSpace(etiNo) && etiNo.Length >= 2
            && string.Compare(etiNo[..2], "ES", true) == 0;

        public static bool CheckEtiIsMotorsSubAssembly(string etiNo) =>
            !string.IsNullOrWhiteSpace(etiNo) && etiNo.Length >= 2
            && string.Compare(etiNo[..2], "SA", true) == 0;

        public static bool CanCreate(long id, string number, out ErrorList errors)
        {
            errors = new();
            if (id < 1)
            {
                errors.Add($"El identificador [{id}] no es válido. El identificador tiene que ser un valor entero positivo.");
            }
            if (string.IsNullOrWhiteSpace(number))
            {
                errors.Add("El número de ETI no puede estar en blanco.");
            }
            return !errors.Any();
        }

        public static Eti Create(long id, string number, string? componentNo, string? revision, string? lotNo, bool isEnabled)
        {
            if (!CanCreate(id, number, out var errors)) throw errors.AsException();
            return new(id, number, componentNo, revision, lotNo, isEnabled);
        }

        private Eti(long id, string number, string? componentNo, string? revision, string? lotNo, bool isEnabled)
        {
            Id = id;
            Number = number;
            ComponentNo = componentNo;
            Revision = revision;
            LotNo = lotNo;
            IsEnabled = isEnabled;
        }

        public long Id { get; }

        public string Number { get; }

        public string? ComponentNo { get; }

        public string? Revision { get; }

        public string? LotNo { get; }

        public bool IsEnabled { get; }

        public bool IsComponent => CheckEtiIsComponent(Number);

        public bool IsAssembly => CheckEtiIsAssembly(Number);

        public bool IsServicePart => CheckEtiIsServicePart(Number);

        public bool IsSubAssembly => CheckEtiIsSubAssembly(Number);

        public bool IsMotorsSubAssembly => CheckEtiIsMotorsSubAssembly(Number);
    }
}