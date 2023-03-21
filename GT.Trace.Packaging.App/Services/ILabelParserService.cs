using GT.Trace.Packaging.Domain.Entities;

namespace GT.Trace.Packaging.App.Services
{
    public interface ILabelParserService
    {
        bool TryParseRiderFormat(string value, out Label? labelData);

        bool TryParseNewWBFormat(string value, out Label? labelData);
    }
}