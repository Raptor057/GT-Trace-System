using GT.Trace.Packaging.Infra.DataSources.Entities;

namespace GT.Trace.Packaging.Infra.DataSources
{
    public class AppsSqlDB
    {
        private readonly DapperSqlDbConnection _con;

        public AppsSqlDB(ConfigurationSqlDbConnection<AppsSqlDB> con)
        {
            _con = con;
        }

        public async Task<dynamic> TryGetUnitByIDAsync(long id) =>
            await _con.QuerySingleAsync<dynamic>("SELECT *, CAST(ISNULL(functest_status, ft_mata_status) AS BIT) [func_test_status] FROM APPS.dbo.pro_tms WHERE id=@id;", new { id })
            .ConfigureAwait(false);

        public async Task<pro_prod_units> GetLineByCodeAsync(string lineCode) =>
            await _con.QuerySingleAsync<pro_prod_units>("SELECT * FROM dbo.pro_prod_units WHERE letter = @lineCode;", new { lineCode })
            .ConfigureAwait(false);

        public async Task<pro_prod_units> GetLineByNameAsync(string lineName) =>
            await _con.QuerySingleAsync<pro_prod_units>("SELECT * FROM dbo.pro_prod_units WHERE comments = @lineName;", new { lineName })
            .ConfigureAwait(false);

        public async Task<pro_prod_units> GetLineByIDAsync(int lineID) =>
            await _con.QuerySingleAsync<pro_prod_units>("SELECT * FROM dbo.pro_prod_units WHERE id = @lineID;", new { lineID })
            .ConfigureAwait(false);

        public async Task<pro_production> GetWorkOrderByLineIDAsync(int lineID) =>
            await _con.QuerySingleAsync<pro_production>(
                "SELECT TOP 1 * FROM dbo.pro_production WHERE id_line = @lineID AND is_stoped = 0 AND is_running = 1 AND is_finished = 0 ORDER BY last_update_time DESC;", new { lineID })
            .ConfigureAwait(false);

        public async Task<pro_production> GetWorkOrderByLineIDAsync(int lineID, string workOrderCode) =>
            await _con.QuerySingleAsync<pro_production>(
                "SELECT TOP 1 * FROM dbo.pro_production WHERE id_line = @lineID AND codew = @workOrderCode AND is_stoped = 0 AND is_running = 1 AND is_finished = 0 ORDER BY last_update_time DESC;",
                new { lineID, workOrderCode })
            .ConfigureAwait(false);

        public async Task<pro_production> GetWorkOrderByCodeAsync(string lineCode) =>
            await _con.QuerySingleAsync<pro_production>(
                "SELECT TOP 1 * FROM dbo.pro_production WHERE codew=@lineCode ORDER BY last_update_time DESC;", new { lineCode })
            .ConfigureAwait(false);

        public async Task IncreaseWorkOrderQuantityByOne(string workOrderCode, int lineID) =>
            await _con.ExecuteAsync("UPDATE dbo.pro_production SET current_qty = current_qty + 1 WHERE codew=@workOrderCode AND id_line = @lineID;", new { lineID, workOrderCode }).ConfigureAwait(false);

        public async Task<long> AddUnitAsync(int position, DateTime creationTime, string partNo, string revision, string ratio, string type, int workOrderID, string lineCode, string serialCode) =>
            await _con.ExecuteScalarAsync<long>(@"INSERT INTO [dbo].[pro_tms]
           ([is_ok]
           ,[is_locked]
           ,[line]
           ,[type]
           ,[id_reference]
           ,[julian_day]
           ,[year]
           ,[serial]
           ,[ratio]
           ,[rev]
           ,[station]
           ,[serial_code]
           ,[created_at]
           ,[last_location]
           ,[ft_mata_date]
           ,[ft_mata_status])
            VALUES
           (@is_ok
           ,@is_locked
           ,@line
           ,@type
           ,@id_reference
           ,@julian_day
           ,@year
           ,@serial
           ,@ratio
           ,@rev
           ,@station
           ,@serial_code
           ,@created_at
           ,@line
           ,@ft_mata_date
           ,@ft_mata_status);
            SELECT SCOPE_IDENTITY();",
                new
                {
                    is_ok = true,
                    is_locked = false,
                    line = lineCode,
                    station = position.ToString(),
                    year = creationTime.Year,
                    julian_day = creationTime.DayOfYear,
                    created_at = creationTime,
                    serial = partNo,
                    rev = revision,
                    ratio = ratio,
                    type = type,
                    id_reference = workOrderID,
                    serial_code = serialCode,
                    ft_mata_date = DateTime.Now,
                    ft_mata_status = true
                }).ConfigureAwait(false);

        public async Task<long?> GetUnitIDBySerialCodeAsync(string serialCode) =>
            await _con.ExecuteScalarAsync<long?>("SELECT id FROM dbo.pro_tms WHERE serial_code = @serialCode;", new { serialCode }).ConfigureAwait(false);

    public async Task<int?> GetActiveWorkOrderByLine(int id_line) =>
            await _con.ExecuteScalarAsync<int?>("SELECT COUNT(codew) as [Orden Activa] FROM dbo.pro_production WHERE id_line=@id_line AND is_running=1 AND is_stoped=0 AND is_finished=0", new { id_line }).ConfigureAwait(false);
    }
}