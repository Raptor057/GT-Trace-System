namespace GT.Trace.Domain.Entities
{
    public sealed class SetComponent
    {
        public SetComponent(string pointOfUseCode, string compNo, string etiNo)
        {
            PointOfUseCode = pointOfUseCode;
            CompNo = compNo;
            EtiNo = etiNo;
        }

        public string PointOfUseCode { get; }

        public string CompNo { get; }

        public string EtiNo { get; }
    }
}