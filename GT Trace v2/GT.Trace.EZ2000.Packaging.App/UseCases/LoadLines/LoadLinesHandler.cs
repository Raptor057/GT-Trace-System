using GT.Trace.Common;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.LoadLines
{
    internal sealed class LoadLinesHandler : ResultInteractorBase<LoadLinesRequest, LoadLinesResponse>
    {
        private readonly ILinesDao _lines;

        public LoadLinesHandler(ILinesDao lines)
        {
            _lines = lines;
        }
        public async override Task<Result<LoadLinesResponse>> Handle(LoadLinesRequest request, CancellationToken cancellationToken)
        {
            return OK(new LoadLinesResponse(await _lines.GetLinesAsync().ConfigureAwait(false)));
        }
    }
}
