namespace GT.Trace.Etis.Domain.Services
{
    public interface IEtiParserService
    {
        bool TryParseEti(string value, out long id, out string no);
    }
}