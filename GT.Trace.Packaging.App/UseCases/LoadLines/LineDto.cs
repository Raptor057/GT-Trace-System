namespace GT.Trace.Packaging.App.UseCases.LoadLines
{
    public record LineDto(
        int ID,
        string Code,
        string Name,
        string ProdType,
        string ProdFamily,
        string LoadedPartNo,
        string LoadedPartRev,
        string LoadedPartDesc,
        string LoadedCodeW,
        int ClientNo,
        string ClientCode,
        string ClientName,
        string ClientPartNo,
        string Vendor,
        int OrderSize,
        int OrderQty,
        int MastersQty,
        int PalletQty,
        int PalletSize,
        int StandardPackSize,
        string MasterTypeCode,
        string Suffix,
        string PartFamily,
        string PONumber,
        string PackTypeCode);
}