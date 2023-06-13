using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using GT.Trace.Etis.Domain.Services;
using Microsoft.Extensions.Logging;

namespace GT.Trace.Etis.App.UseCases.ParseEti
{
    internal sealed class ParseEtiIDHandler : ResultInteractorBase<ParseEtiRequest, ParseEtiResponse>
    {
        private readonly IEtiParserService _parser;

        private readonly ILogger<ParseEtiIDHandler> _logger;

        public ParseEtiIDHandler(IEtiParserService parser, ILogger<ParseEtiIDHandler> logger)
        {
            _parser = parser;
            _logger = logger;
        }

        #pragma warning disable CS1998
        public override async Task<Result<ParseEtiResponse>> Handle(ParseEtiRequest request, CancellationToken cancellationToken)
        {
            if (!_parser.TryParseEti(request.EtiInput!, out var etiID, out var etiNo))
            {
                return Fail($"Ocurrió un problema al procesar el escaneo \"{request.EtiInput}\".");
            }

            return OK(new ParseEtiResponse(etiID, etiNo));
        }
    }
}