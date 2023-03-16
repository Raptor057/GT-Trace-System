using GT.Trace.Common;
using GT.Trace.EtiMovements.Domain.Enums;

namespace GT.Trace.EtiMovements.Domain.Entities
{
    public sealed class EtiMovement
    {
        public static bool CanCreate(string pointOfUseCode, string etiNo, string componentNo, out ErrorList errors)
        {
            errors = new();
            if (string.IsNullOrWhiteSpace(pointOfUseCode))
            {
                errors.Add("El túnel es requerido pero se encuentra en blanco.");
            }
            if (string.IsNullOrWhiteSpace(etiNo))
            {
                errors.Add("El número de ETI es requerido pero se encuentra en blanco.");
            }
            if (string.IsNullOrWhiteSpace(componentNo))
            {
                errors.Add("El número de componente es requerido pero se encuentra en blanco.");
            }
            return errors.IsEmpty;
        }

        public static EtiMovement Create(string pointOfUseCode, string etiNo, string componentNo, DateTime? utcStartTime, DateTime? utcUsageTime, DateTime? utcEndTime, bool isDepleted)
        {
            if (!CanCreate(pointOfUseCode, etiNo, componentNo, out var errors)) throw errors.AsException();
            return new(pointOfUseCode, etiNo, componentNo, utcStartTime, utcUsageTime, utcEndTime, isDepleted);
        }

        private EtiMovement(string pointOfUseCode, string etiNo, string componentNo, DateTime? utcStartTime, DateTime? utcUsageTime, DateTime? utcEndTime, bool isDepleted)
        {
            PointOfUseCode = pointOfUseCode;
            EtiNo = etiNo;
            ComponentNo = componentNo;
            StartTime = utcStartTime?.ToLocalTime();
            UsageTime = utcUsageTime?.ToLocalTime();
            EndTime = utcEndTime?.ToLocalTime();
            IsDepleted = isDepleted;
        }

        public string PointOfUseCode { get; set; }

        public string EtiNo { get; set; }

        public string ComponentNo { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? UsageTime { get; set; }

        public bool IsDepleted { get; set; }

        public MovementTypes MovementType =>
            StartTime.HasValue && !UsageTime.HasValue && !EndTime.HasValue
                ? MovementTypes.Load
                : StartTime.HasValue && UsageTime.HasValue && !EndTime.HasValue
                    ? MovementTypes.Use
                    : StartTime.HasValue && !UsageTime.HasValue && EndTime.HasValue
                        ? MovementTypes.Unload
                        : StartTime.HasValue && UsageTime.HasValue && EndTime.HasValue
                            ? MovementTypes.Return
                            : throw new InvalidOperationException("Tipo de movimiento de ETI no reconicido.");
    }
}