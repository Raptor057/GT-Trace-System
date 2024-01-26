using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.BomSnapShot.App.UseCases.SaveSnapshot
{
    public sealed class SaveSnapshotRequest:IRequest<SaveSnapshotResponse>
    {
        public static bool CanCreate(string pointOfUseCode, out ErrorList errors)
        {
            errors = new();
            if (string.IsNullOrEmpty(pointOfUseCode))
            {
                errors.Add("pointOfUseCode Sin Declarar");
            }
            return errors.IsEmpty;
        }
        public static SaveSnapshotRequest Create(string pointOfUseCode)
        {
            if (!CanCreate(pointOfUseCode, out var errors)) throw errors.AsException();
            return new(pointOfUseCode);
        }
        private SaveSnapshotRequest(string pointOfUseCode)
        {
            PointOfUseCode = pointOfUseCode;
        }
        public string PointOfUseCode { get; }
    }
}
