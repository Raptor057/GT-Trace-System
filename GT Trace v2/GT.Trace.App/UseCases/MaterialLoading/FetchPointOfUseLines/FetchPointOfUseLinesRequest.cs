using GT.Trace.Common.CleanArch;

namespace GT.Trace.App.UseCases.MaterialLoading.FetchPointOfUseLines
{
    public class FetchPointOfUseLinesRequest : IResultRequest<FetchPointOfUseLinesResponse>
    {
        public static bool CanCreate(string pointOfUseCode, out List<string> errors)
        {
            errors = new();
            if (string.IsNullOrWhiteSpace(pointOfUseCode))
            {
                errors.Add("El código de túnel es obligatorio pero se encuentra en blanco.");
            }
            return errors.Count == 0;
        }

        public static FetchPointOfUseLinesRequest Create(string pointOfUseCode)
        {
            return new(pointOfUseCode);
        }

        private FetchPointOfUseLinesRequest(string pointOfUseCode)
        {
            PointOfUseCode = pointOfUseCode;
        }

        public string PointOfUseCode { get; }
    }
}