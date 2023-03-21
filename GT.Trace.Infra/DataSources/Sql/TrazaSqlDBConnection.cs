using GT.Trace.Infra.Entities;

namespace GT.Trace.Infra.DataSources.Sql
{
    internal class TrazaSqlDBConnection : DisposableDapperDatabaseConnection<TrazaConfigurableSqlDatabaseConnectionFactory>, ITrazaSqlDBConnection
    {
        public TrazaSqlDBConnection(TrazaConfigurableSqlDatabaseConnectionFactory connections)
            : base(connections)
        { }

        public async Task UnloadMaterialAsync(string etiNo, string componentNo, string lineCode, DateTime utcTimeStamp)
        {
            const string delete = "DELETE FROM TBL_POINT_USE WHERE ETI_no=@etiNo;";
            const string insert = "INSERT INTO TZ_TBL_ADJUST_TUNEL_HIST (ETI_NO,COMPONENTE,FECHA,HORA,LINEA) values (@etiNo,@componentNo,FORMAT(@utcTimeStamp, 'dd-MMM-yyyy'),FORMAT(@utcTimeStamp, 'HH:mm'),@lineCode);";
            await ExecuteAsync(delete, new { etiNo }).ConfigureAwait(false);
            await ExecuteAsync(insert, new { etiNo, componentNo, lineCode, utcTimeStamp }).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Tbl_Point_use>> FetchLoadedEtisAsync(string lineCode, string partNo) =>
            await QueryAsync<Tbl_Point_use>("SELECT * FROM tbl_Point_use WHERE linea = @lineCode AND is_used = 0 AND NP_final = @partNo;", new { lineCode, partNo })
            .ConfigureAwait(false);

        public async Task<IEnumerable<Etis_WB>> GetActiveLineSetAsync(string lineName) =>
            await QueryAsync<Etis_WB>("SELECT * FROM dbo.Etis_WB WITH(NOLOCK) WHERE linea = @lineName AND status = 0;", new { lineName });

        public async Task<Groups_Mails> GetGroupByNameAsync(string name) =>
            await QuerySingleAsync<Groups_Mails>("SELECT * FROM Groups_Mails WHERE GROUP_NAME=@name;", new { name })
            .ConfigureAwait(false);

        public async Task<long> SaveEtiInPointOfUse(string lineCode, string workOrderCode, string partNo, long folio, string lineOrder, string order, string etiNo, string pointOfUseCode, string operatorNo,
            string lotNo, string componentNo, string category) =>
             await ExecuteScalarAsync<long>(@"INSERT INTO tbl_point_use (
                    linea, codew, np_final, folio, linea_orden, orden, ETI_no, punto_uso, operador,fecha, hora,lote,componente,COMMENTS
                ) VALUES (
                    @linea, @codew, @NP_final, @Folio, @Linea_Orden, @Orden, @ETI_no, @Punto_uso, @Operador, FORMAT(GETUTCDATE(), 'dd-MMM-yyyy'), FORMAT(GETUTCDATE(), 'HH:mm:ss'), @LOTE, @Componente, @comments
                ); SELECT CAST(SCOPE_IDENTITY() AS BIGINT);",
                new
                {
                    linea = lineCode,
                    codew = workOrderCode,
                    NP_final = partNo,
                    Folio = folio,
                    Linea_Orden = lineOrder,
                    Orden = order,
                    ETI_no = etiNo,
                    Punto_uso = pointOfUseCode,
                    Operador = operatorNo,
                    LOTE = lotNo,
                    Componente = componentNo,
                    comments = category
                })
                .ConfigureAwait(false);
    }
}