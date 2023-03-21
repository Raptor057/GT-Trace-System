using GT.Trace.EZ2000.Packaging.Domain.Entities;


namespace GT.Trace.EZ2000.Packaging.App.Services
{
    public interface ILabelParserService
    {
        /// <summary>
        /// Interface para RiderFormat
        /// </summary>
        /// <param name="value"></param>
        /// <param name="labelData"></param>
        /// <returns></returns>
        bool TryParseRiderFormat(string value, out Label? labelData);
        /// <summary>
        /// Interface para WB
        /// </summary>
        /// <param name="value"></param>
        /// <param name="labelData"></param>
        /// <returns></returns>
        bool TryParseNewWBFormat(string value, out Label? labelData);
    }
}
