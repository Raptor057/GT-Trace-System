using GT.Trace.Domain.Entities;

namespace GT.Trace.Domain.Services
{
    public interface ILabelParserService
    {
        bool TryParseRiderFormat(string value, out Label? labelData);

        bool TryParseNewWBFormat(string value, out Label? labelData);

        bool TryParseEti(string value, out long? id, out string? no);
    }
}