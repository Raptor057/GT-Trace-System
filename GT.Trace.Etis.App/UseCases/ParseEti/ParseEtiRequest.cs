using GT.Trace.Common.CleanArch;

namespace GT.Trace.Etis.App.UseCases.ParseEti
{
    public sealed record ParseEtiRequest(string EtiInput) : IResultRequest<ParseEtiResponse>;
}