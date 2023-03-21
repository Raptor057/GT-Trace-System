using GT.Trace.Common.CleanArch;

namespace GT.Trace.App.UseCases.MaterialLoading.FetchEtiPointsOfUse
{
    public sealed class FetchEtiPointsOfUseRequest : IResultRequest<FetchEtiPointsOfUseResponse>
    {
        public static bool CanCreate(string lineCode, string partNo, string etiNo, out List<string> errors)
        {
            errors = new();
            if (string.IsNullOrWhiteSpace(lineCode))
            {
                errors.Add("El código de línea es obligatorio y se encuentra en blanco.");
            }
            if (string.IsNullOrWhiteSpace(partNo))
            {
                errors.Add("El número de parte es obligatorio y se encuentra en blanco.");
            }
            if (string.IsNullOrWhiteSpace(etiNo))
            {
                errors.Add("El número de ETI es obligatorio y se encuentra en blanco.");
            }
            return errors.Count == 0;
        }

        public static FetchEtiPointsOfUseRequest Create(string lineCode, string partNo, string etiNo)
        {
            return new(lineCode, partNo, etiNo);
        }

        private FetchEtiPointsOfUseRequest(string lineCode, string partNo, string etiNo)
        {
            PartNo = partNo;
            EtiNo = etiNo;
            LineCode = lineCode;
        }

        public string PartNo { get; }

        public string EtiNo { get; }

        public string LineCode { get; }
    }
}