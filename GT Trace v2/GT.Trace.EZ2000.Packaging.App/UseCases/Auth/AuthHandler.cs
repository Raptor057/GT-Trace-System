using GT.Trace.Common.CleanArch;
using GT.Trace.Common;

namespace GT.Trace.EZ2000.Packaging.App.UseCases.Auth
{
    internal sealed class AuthHandler : ResultInteractorBase<AuthRequest, AuthResponse>
    {
        private readonly IUsersDao _users;

        public AuthHandler(IUsersDao users)
        {
            _users = users;
        }

        public override async Task<Result<AuthResponse>> Handle(AuthRequest request, CancellationToken cancellationToken)
        {
            var isSupervisorUser = await _users.IsSupervisorUser(request.AuthorizedUserPassword).ConfigureAwait(false);

            var response = new AuthResponse(isSupervisorUser);
            return OK(response);
        }
    }
}