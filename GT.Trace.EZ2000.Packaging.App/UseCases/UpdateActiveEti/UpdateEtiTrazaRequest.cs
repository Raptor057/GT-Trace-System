using GT.Trace.Common.CleanArch;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.UpdateActiveEti
{
    public sealed record UpdateEtiTrazaRequest(string etiNo) :IRequest<UpdateEtiTrazaResponse>;
}
