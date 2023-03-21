using GT.Trace.Common.CleanArch;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.Auth
{
    public record AuthRequest(string AuthorizedUserPassword): IResultRequest<AuthResponse>;
}
