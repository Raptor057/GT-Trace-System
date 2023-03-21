//using GT.Trace.Common.Infra;

//namespace GT.Trace.EtiMovements.Infra.Daos
//{
//    internal class UpdateEtiTrazaDao : BaseDao
//    {
//        public UpdateEtiTrazaDao(IGttSqlDBConnection connection)
//            : base(connection)
//        { }
//        public async Task UpdateEtiTrazaAsync(string etiNo)
//        {
//            await Connection.ExecuteAsync("EXEC [dbo].[UpsUpdateActiveEtis] @EtiNo", new { etiNo }).ConfigureAwait(false);
//        }
//    }
//}
