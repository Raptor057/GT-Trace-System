using GT.Trace.Common;
//Ver este metodo a detalle.
namespace GT.Trace.Domain.Entities
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
            return errors.IsEmpty;
        }

        public static Eti Create(long id, string number, Part component, string? lotNo, bool isEnabled, EtiStatus? etiStatus)
        {
            if (!CanCreate(id, number, out var errors)) throw errors.AsException();
            return new(id, number, component, lotNo, isEnabled, etiStatus);
        }

        private Eti(long id, string number, Part component, string? lotNo, bool isEnabled, EtiStatus? etiStatus)
        {
            Id = id;
            Number = number;
            Component = component;
            LotNo = lotNo;
            IsEnabled = isEnabled;
            Status = etiStatus;
        }

        public long Id { get; }

        public string Number { get; }

        public Part Component { get; }

        public string? LotNo { get; }

        public bool IsEnabled { get; }

        public EtiStatus? Status { get; private set; }

        public bool IsLoaded => Status != null;

        public bool IsUsed => Status != null && Status.UsageTime.HasValue && Status.IsDepleted;

        public bool IsActive => Status != null && Status.UsageTime.HasValue && !Status.ExpirationTime.HasValue;

        public bool IsGood => Status == null;

        public void SetLoadStatus(string pointOfUseCode)
        {
            Status = new EtiStatus(pointOfUseCode, DateTime.Now, null, null, false);
        }

        public void ClearStatus()
        {
            Status = null;
        }
    }
}