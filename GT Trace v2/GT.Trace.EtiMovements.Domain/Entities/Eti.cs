using GT.Trace.Common;
using GT.Trace.EtiMovements.Domain.Enums;

namespace GT.Trace.EtiMovements.Domain.Entities
{
    public sealed class Eti
    {
        public static bool CanCreate(string? etiNo, string? componentNo, string? lotNo, Revision? revision, out ErrorList errors)
        {
            errors = new();
            if (string.IsNullOrWhiteSpace(etiNo))
            {
                errors.Add("El número de ETI es requerido pero se encuentra en blanco.");
            }
            if (string.IsNullOrWhiteSpace(lotNo))
            {
                errors.Add("El número de lote es requerido pero se encuentra en blanco.");
            }
            if (string.IsNullOrWhiteSpace(componentNo))
            {
                errors.Add("El número de componente es requerido pero se encuentra en blanco.");
            }
            if (revision == null)
            {
                errors.Add("El número de revisión es requerido pero se encuentra en blanco.");
            }
            return errors.IsEmpty;
        }

        public static Eti Create(string? etiNo, string? componentNo, string? lotNo, Revision? revision, bool isShared, EtiMovement? lastMovement)
        {
            if (!CanCreate(etiNo, componentNo, lotNo, revision, out var errors)) throw errors.AsException();
            return new(etiNo!, componentNo!, lotNo!, revision!, isShared, lastMovement);
        }

        private Eti(string number, string componentNo, string lotNo, Revision revision, bool isShared, EtiMovement? lastMovement)
        {
            Number = number;
            ComponentNo = componentNo;
            Revision = revision;
            LastMovement = lastMovement;
            LotNo = lotNo;
            IsShared = isShared;
        }

        public bool IsShared { get; }

        public string LotNo { get; }

        public string Number { get; }

        public string ComponentNo { get; }

        public Revision Revision { get; }

        public EtiMovement? LastMovement { get; private set; }

        public bool IsLoaded => LastMovement?.MovementType == MovementTypes.Load;

        public bool IsActive => LastMovement?.MovementType == MovementTypes.Use;

        public bool IsAvailable => LastMovement == null || ((LastMovement.MovementType == MovementTypes.Return || LastMovement.MovementType == MovementTypes.Unload) && !LastMovement.IsDepleted);

        public bool CanLoad(string pointOfUseCode, out ErrorList errors)
        {
            errors = new();
            #region #region Codigo Comentado por Ing. MJV - ????
            //if (!IsAvailable)
            //{
            //    errors.Add($"No se puede cargar la ETI {Number} ya que no se encuentra libre.");
            //}
            #endregion
            if (string.IsNullOrWhiteSpace(pointOfUseCode))
            {
                errors.Add("El túnel es requerido pero se encuentra en blanco.");
            }
            if (IsActive)
            {
                errors.Add($"ETI {Number} se encuentra en uso en el túnel {LastMovement?.PointOfUseCode} desde {LastMovement?.UsageTime:el \\día dd-MMM-yyyy a la\\s HH:mm \\hora\\s}.");
            }
            else if (IsLoaded)
            {
                errors.Add($"ETI {Number} se encuentra cargada en el túnel {LastMovement?.PointOfUseCode} desde {LastMovement?.StartTime:el \\día dd-MMM-yyyy a la\\s HH:mm \\hora\\s}.");
            }
            return errors.IsEmpty;
        }

        public void Load(string pointOfUseCode)
        {
            if (!CanLoad(pointOfUseCode, out var errors)) throw errors.AsException();

            LastMovement = EtiMovement.Create(
                pointOfUseCode,
                Number,
                ComponentNo,
                DateTime.Now,
                null,
                null,
                false);
        }

        public bool CanUnload(out ErrorList errors)
        {
            errors = new();

            if (IsActive)
            {
                errors.Add($"ETI {Number} se encuentra en uso en el túnel {LastMovement?.PointOfUseCode} desde {LastMovement?.UsageTime:el \\día dd-MMM-yyyy a la\\s HH:mm \\hora\\s}.");
            }
            else if (IsAvailable && LastMovement != null)
            {
                errors.Add($"ETI {Number} se descargó del túnel {LastMovement?.PointOfUseCode} desde {LastMovement?.EndTime:el \\día dd-MMM-yyyy a la\\s HH:mm \\hora\\s}.");
            }
            else if (!IsLoaded)
            {
                errors.Add($"No se puede descargar la ETI {Number} ya que no se encuentra cargada.");
            }
            #region Codigo comendato por Ing. RAG - 2/24/2023
            //Esto se cambio para evitar confion entre mensajes ya que retornaba 2 mensajes
            //RAG - 2/24/2023
            //if (!IsLoaded)
            //{
            //    errors.Add($"No se puede descargar la ETI {Number} ya que no se encuentra cargada.");
            //}
            //if (IsActive)
            //{
            //    errors.Add($"ETI {Number} se encuentra en uso en el túnel {LastMovement?.PointOfUseCode} desde {LastMovement?.UsageTime:el \\día dd-MMM-yyyy a la\\s HH:mm \\hora\\s}.");
            //}
            //else if (IsAvailable && LastMovement != null)
            //{
            //    errors.Add($"ETI {Number} se descargó del túnel {LastMovement?.PointOfUseCode} desde {LastMovement?.EndTime:el \\día dd-MMM-yyyy a la\\s HH:mm \\hora\\s}.");
            //}
            #endregion
            return errors.IsEmpty;
        }

        public void Unload()
        {
            if (!CanUnload(out var errors)) throw errors.AsException();

            LastMovement = EtiMovement.Create(
                LastMovement!.PointOfUseCode,
                LastMovement!.EtiNo,
                LastMovement!.ComponentNo,
                LastMovement!.StartTime,
                LastMovement!.UsageTime,
                DateTime.Now,
                LastMovement!.IsDepleted);
        }

        public bool CanUse(out ErrorList errors)
        {
            errors = new();
            if (IsActive)
            {
                errors.Add($"ETI {Number} se encuentra en uso en el túnel {LastMovement?.PointOfUseCode} desde {LastMovement?.UsageTime:el \\día dd-MMM-yyyy a la\\s HH:mm \\hora\\s}.");
            }
            else if (IsAvailable && LastMovement != null)
            {
                errors.Add($"ETI {Number} se descargó del túnel {LastMovement?.PointOfUseCode} desde {LastMovement?.EndTime:el \\día dd-MMM-yyyy a la\\s HH:mm \\hora\\s}.");
            }
            else if (!IsLoaded)
            {
                errors.Add($"No se puede utilizar la ETI {Number} ya que no se encuentra cargada.");
            }
            #region Codigo comendato por Ing. RAG - 2/24/2023
            //Este metodo hace que aparezcan 2 mensajes a la vez cuando se escanea 2 veces una etiqueta
            //que ya esta cargada aparece
            //No se puede utilizar la ETI {Number} ya que no se encuentra cargada.
            //RAG - 2/24/2023
            //ETI {Number} se encuentra en uso en el túnel {LastMovement?.PointOfUseCode} desde {LastMovement?.UsageTime:el \\día dd-MMM-yyyy a la\\s HH:mm \\hora\\s}.
            //if (!IsLoaded)
            //{
            //    errors.Add($"No se puede utilizar la ETI {Number} ya que no se encuentra cargada.(CanUse)");
            //}
            //if (IsActive)
            //{
            //    errors.Add($"ETI {Number} se encuentra en uso en el túnel {LastMovement?.PointOfUseCode} desde {LastMovement?.UsageTime:el \\día dd-MMM-yyyy a la\\s HH:mm \\hora\\s}.");
            //}
            //else if (IsAvailable && LastMovement != null)
            //{
            //    errors.Add($"ETI {Number} se descargó del túnel {LastMovement?.PointOfUseCode} desde {LastMovement?.EndTime:el \\día dd-MMM-yyyy a la\\s HH:mm \\hora\\s}.");
            //}
            #endregion
            return errors.IsEmpty;
        }

        public void Use()
        {
            if (!CanUse(out var errors)) throw errors.AsException();

            LastMovement = EtiMovement.Create(
                LastMovement!.PointOfUseCode,
                LastMovement!.EtiNo,
                LastMovement!.ComponentNo,
                LastMovement!.StartTime,
                DateTime.Now,
                LastMovement!.EndTime,
                LastMovement!.IsDepleted);
        }

        public bool CanReturn(bool isChangeOver, out ErrorList errors)
        {
            errors = new();
            #region Codigo Comentado por Ing. MJV - ????
            //Fecha: ????
            //Nota: esto lo dejo el ingeniero marcos, comentado, yo solo le agregue el region, quien lo dejo comentado y la fecha
            //como ???? ya que no hay un registro de cuando dejo eso comentado.
            //Rag- 02/24/2029
            // ! Esto para poder retornar ETIs que no estan activas ya que se necesita la etiqueta de retorno tambien
            // para las ETIs carganas que no estan en uso.
            //if (!IsActive)
            //{
            //    errors.Add($"No se puede retornar la ETI {Number} ya que no se encuentra en uso.");
            //}
            //if (IsLoaded)
            //{
            //    errors.Add($"ETI {Number} se cargó en el túnel {LastMovement?.PointOfUseCode} desde {LastMovement?.StartTime:el \\día dd-MMM-yyyy a la\\s HH:mm \\hora\\s}.");
            //}
            //else
            #endregion
            if (IsShared && isChangeOver)
            {
                errors.Add($"ETI {Number} se encuentra compartida entre líneas.");
            }
            if (IsAvailable && LastMovement != null)
            {
                errors.Add($"ETI {Number} no se encuentra cargada. Se descargó del túnel {LastMovement?.PointOfUseCode} desde {LastMovement?.EndTime:el \\día dd-MMM-yyyy a la\\s HH:mm \\hora\\s}.");
            }
            return errors.IsEmpty;
        }

        public void Return(bool isChangeOver)
        {
            if (!CanReturn(isChangeOver, out var errors)) throw errors.AsException();

            LastMovement = EtiMovement.Create(
                LastMovement!.PointOfUseCode,
                LastMovement!.EtiNo,
                LastMovement!.ComponentNo,
                LastMovement!.StartTime,
                LastMovement!.UsageTime,
                DateTime.Now,
                LastMovement!.IsDepleted);
        }
    }
}