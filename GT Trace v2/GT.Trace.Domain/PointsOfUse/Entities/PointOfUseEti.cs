using GT.Trace.Domain.PointsOfUse.Events;

namespace GT.Trace.Domain.PointsOfUse.Entities
{
    public sealed class PointOfUseEti
    {
        public static bool CanCreate(string pointOfUseCode, string etiNo, string componentNo, out ErrorList errors)
        {
            errors = new();
            if (string.IsNullOrWhiteSpace(pointOfUseCode))
            {
                errors.Add("El código del túnel no puede estar en blanco.");
            }
            if (string.IsNullOrWhiteSpace(etiNo))
            {
                errors.Add("El número de ETI no puede estar en blanco.");
            }
            if (string.IsNullOrWhiteSpace(componentNo))
            {
                errors.Add("El número de componente no puede estar en blanco.");
            }
            return !errors.Any();
        }

        public static PointOfUseEti Create(string pointOfUseCode, string etiNo, string componentNo, DateTime? utcEffectiveTime, DateTime? utcExpirationTime, DateTime? utcUsageTime, bool isDepleted)
        {
            if (!CanCreate(pointOfUseCode, etiNo, componentNo, out var errors)) throw errors.AsException();
            return new(pointOfUseCode, etiNo, componentNo, utcEffectiveTime?.ToLocalTime(), utcExpirationTime?.ToLocalTime(), utcUsageTime?.ToLocalTime(), isDepleted);
        }

        private readonly Queue<object> _events = new();

        private PointOfUseEti(string pointOfUseCode, string etiNo, string componentNo, DateTime? effectiveTime, DateTime? expirationTime, DateTime? usageTime, bool isDepleted)
        {
            PointOfUseCode = pointOfUseCode;
            EtiNo = etiNo;
            ComponentNo = componentNo;
            EffectiveTime = effectiveTime;
            ExpirationTime = expirationTime;
            UsageTime = usageTime;
            IsDepleted = isDepleted;
        }

        public string PointOfUseCode { get; }

        public string EtiNo { get; }

        public string ComponentNo { get; }

        public DateTime? EffectiveTime { get; private set; }

        public DateTime? ExpirationTime { get; private set; }

        public DateTime? UsageTime { get; private set; }

        public bool IsDepleted { get; private set; }

        public bool IsNew => !EffectiveTime.HasValue && !UsageTime.HasValue && !ExpirationTime.HasValue;

        public bool IsLoaded => EffectiveTime.HasValue && !UsageTime.HasValue && !ExpirationTime.HasValue;

        public bool IsUnloaded => EffectiveTime.HasValue && !UsageTime.HasValue && ExpirationTime.HasValue;

        public bool IsUsed => EffectiveTime.HasValue && UsageTime.HasValue && !ExpirationTime.HasValue;

        public bool IsReturned => EffectiveTime.HasValue && UsageTime.HasValue && ExpirationTime.HasValue;

        public EtiStatus Status =>
            IsNew || IsUnloaded || IsReturned
                ? EtiStatus.Available
                : IsLoaded
                    ? EtiStatus.Loaded
                    : IsUsed
                        ? EtiStatus.Active
                        : throw new Exception("Estatus desconocido.");

        public bool CanLoad(out ErrorList errors)
        {
            errors = new();
            if (Status != EtiStatus.Available)
            {
                // loaded and used but not returned
                if (IsUsed)
                {
                    errors.Add($"ETI {EtiNo} se encuentra en uso en el túnel {PointOfUseCode} desde {UsageTime:el \\día dd-MMM-yyyy a la\\s HH:mm \\hora\\s}.");
                }
                // loaded, not unloaded nor returned
                if (IsLoaded)
                {
                    errors.Add($"ETI {EtiNo} se encuentra cargada en el túnel {PointOfUseCode} desde {EffectiveTime:el \\día dd-MMM-yyyy a la\\s HH:mm \\hora\\s}.");
                }
            }
            return !errors.Any();
        }

        public void Load()
        {
            if (!CanLoad(out var errors)) throw errors.AsException();
            EffectiveTime = DateTime.Now;
            _events.Enqueue(new EtiLoadedEvent(EtiNo, ComponentNo, PointOfUseCode, EffectiveTime.Value));
        }

        public bool CanUnload(out ErrorList errors)
        {
            errors = new();
            if (Status != EtiStatus.Loaded)
            {
                // loaded and used but not returned
                if (IsUsed)
                {
                    errors.Add($"ETI {EtiNo} se encuentra en uso en el túnel {PointOfUseCode} desde {UsageTime:el \\día dd-MMM-yyyy a la\\s HH:mm \\hora\\s}.");
                }
                // loaded and unloaded
                if (IsUnloaded || IsReturned)
                {
                    errors.Add($"ETI {EtiNo} no se encuentra cargada en el túnel {PointOfUseCode} desde {ExpirationTime:el \\día dd-MMM-yyyy a la\\s HH:mm \\hora\\s}.");
                }
                else if (IsNew)
                {
                    errors.Add($"ETI {EtiNo} no se encuentra cargada en el túnel {PointOfUseCode}.");
                }
            }
            return !errors.Any();
        }

        public void Unload()
        {
            if (!CanUnload(out var errors)) throw errors.AsException();
            ExpirationTime = DateTime.Now;
            _events.Enqueue(new EtiUnloadedEvent(EtiNo, ExpirationTime.Value));
        }

        public bool CanUse(out ErrorList errors)
        {
            errors = new();
            if (Status != EtiStatus.Loaded)
            {
                if (IsUsed)
                {
                    errors.Add($"ETI {EtiNo} se encuentra en uso en el túnel {PointOfUseCode} desde {UsageTime:el \\día dd-MMM-yyyy a la\\s HH:mm \\hora\\s}.");
                }
                // loaded and unloaded
                if (IsUnloaded || IsReturned)
                {
                    errors.Add($"ETI {EtiNo} no se encuentra cargada en el túnel {PointOfUseCode} desde {ExpirationTime:el \\día dd-MMM-yyyy a la\\s HH:mm \\hora\\s}.");
                }
                else if (IsNew)
                {
                    errors.Add($"ETI {EtiNo} no se encuentra cargada en el túnel {PointOfUseCode}.");
                }
            }
            return !errors.Any();
        }

        public void Use()
        {
            if (!CanUse(out var errors)) throw errors.AsException();
            UsageTime = DateTime.Now;
            _events.Enqueue(new EtiUsedEvent(EtiNo, UsageTime.Value));
        }

        public bool CanReturn(out ErrorList errors)
        {
            errors = new();
            if (Status != EtiStatus.Active)
            {
                errors.Add($"ETI {EtiNo} no se puede retornar del túnel {PointOfUseCode} ya que no se está utilizando.");
            }
            return !errors.Any();
        }

        public void Return()
        {
            if (!CanReturn(out var errors)) throw errors.AsException();
            ExpirationTime = DateTime.Now;
            _events.Enqueue(new EtiReturnedEvent(EtiNo, false, ExpirationTime.Value));
        }

        public IReadOnlyList<object> GetEvents() => _events.ToList();
    }
}