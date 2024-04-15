using GT.Trace.ConfigurationPanel.Domain.Repositories;
using GT.Trace.ConfigurationPanel.Infra.DataSources;

namespace GT.Trace.ConfigurationPanel.Infra.Repositories
{
    internal class LineRepository : ILineRepository
    {
        private readonly AppsSqlDB _apps;

        public LineRepository( AppsSqlDB apps)
        {
            _apps = apps;
        }

        /// <summary>
        /// Consulta las lineas activas
        /// </summary>
        /// <returns>
        /// Retorna las lista de lineas activas en GT-APPS
        /// </returns>
        public async Task GetActiveLinesAsync()=>
         await _apps.GetActiveLinesAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codew"></param>
        /// <param name="dblid"></param>
        /// <returns></returns>
        public async Task UpdatedblidAsync(string codew, int dblid)=>
           await _apps.UpdatedblidAsync(codew, dblid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codew"></param>
        /// <param name="stdpack"></param>
        /// <returns></returns>
        public async Task UpdatestdpackAsync(string codew, int stdpack)=>
           await _apps.UpdatestdpackAsync(codew, stdpack);
    }
}
