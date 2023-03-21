using GT.Trace.Packaging.App.UseCases.LoadLines;
using GT.Trace.Packaging.Infra.DataSources;

namespace GT.Trace.Packaging.Infra.Daos.LoadLines
{
    internal record LinesSqlDao(ConfigurationSqlDbConnection<AppsSqlDB> Connection) : ILinesDao
    {
        public async Task<IEnumerable<LineDto>> GetLinesAsync()
        {
            return await Connection.QueryAsync<LineDto>(@"SELECT
    ID
    , Code
    , Name
    , ProdType
    , ProdFamily
    , LoadedPartNo
    , LoadedPartRev
    , LoadedPartDesc
    , LoadedCodeW
    , ClientNo
    , ClientCode
    , ClientName
    , ClientPartNo
    , Vendor
    , OrderSize
    , OrderQty
    , MastersQty
    , PalletQty
    , PalletSize
    , StandardPackSize
    , MasterTypeCode
    , Suffix
    , PartFamily
    , PONumber
    , PackTypeCode
FROM dbo.uvw_production_lines
ORDER BY Code ASC, LastUpdateTime DESC;").ConfigureAwait(false);
        }
    }
}