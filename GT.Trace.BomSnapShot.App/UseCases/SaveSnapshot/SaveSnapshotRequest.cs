using GT.Trace.App.UseCases.MaterialLoading.FetchPointOfUseLines;
using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using MediatR;
using System.ComponentModel;

namespace GT.Trace.BomSnapShot.App.UseCases.SaveSnapshot
{
    public sealed class SaveSnapshotRequest:Common.CleanArch.IRequest<SaveSnapshotResponse>
    {
        public static bool CanCreate(string etiNo,string lineCode, out ErrorList errors)
        {
            errors = new();
            if (string.IsNullOrEmpty(etiNo))
            {
                errors.Add("Etiqueta vacia");
            }
            if (string.IsNullOrEmpty(lineCode))
            {
                errors.Add("Linea vacia");
            }
            return errors.IsEmpty;
        }
        public static SaveSnapshotRequest Create(string etiNo,string lineCode)
        {
            if (!CanCreate(etiNo,lineCode, out var errors)) throw errors.AsException();
            return new(etiNo, lineCode);
        }
        private SaveSnapshotRequest(string etiNo, string lineCode)
        {
            EtiNo = etiNo;
            LineCode=lineCode;
        }
        public string LineCode {  get;}
        public string EtiNo { get; }
    }
}
