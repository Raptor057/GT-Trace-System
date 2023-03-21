using GT.Trace.Common.CleanArch;

namespace GT.Trace.Packaging.App.UseCases.Auth
{
    public record AuthRequest(string AuthorizedUserPassword) : IResultRequest<AuthResponse>;
}