using GT.Trace.App.UseCases.MaterialLoading.FetchPointOfUseLines;
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using MediatR;
using System.ComponentModel;

namespace GT.Trace.BomSnapShot.App.UseCases.SaveSnapshot
{
    public sealed class SaveSnapshotRequest:Common.CleanArch.IRequest<SaveSnapshotResponse>
    {
        public static bool CanCreate(string pointOfUseCode,string componentNo, out ErrorList errors)
        {
            errors = new();
            if (string.IsNullOrEmpty(pointOfUseCode))
            {
                errors.Add("pointOfUseCode Sin Declarar");
            }
            if (string.IsNullOrEmpty(componentNo))
            {
                errors.Add("componentNo Sin declarar");
            }
            return errors.IsEmpty;
        }
        public static SaveSnapshotRequest Create(string pointOfUseCode,string componentNo)
        {
            if (!CanCreate(pointOfUseCode, componentNo, out var errors)) throw errors.AsException();
            return new(pointOfUseCode, componentNo);
        }
        private SaveSnapshotRequest(string pointOfUseCode,string componentNo)
        {
            PointOfUseCode = pointOfUseCode;
            PomponentNo = componentNo;
        }
        public string PointOfUseCode { get; }
        public string PomponentNo { get; }
    }
}
