using GT.Trace.Common;
using GT.Trace.Common.CleanArch;
using System.Diagnostics.Contracts;

namespace GT.Trace.Packaging.App.UseCases.Auth
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
            var Pass = await _users.GetPasswordIsFromEnabledSupervisorUser(request.AuthorizedUserPassword).ConfigureAwait(false);
            if (Pass == false)
            {
                return OK(new AuthResponse(false));
            }
            //if (string.IsNullOrWhiteSpace(request.AuthorizedUserPassword))
            //{
            //    return Fail("La contraseña no puede estar en blanco.");
            //}
            //else if (!await _users.GetPasswordIsFromEnabledSupervisorUser(request.AuthorizedUserPassword).ConfigureAwait(false))
            //{
            //    return Fail("La contraseña no pertenece a un usuario autorizado.");
            //}
            return OK(new AuthResponse(Pass));
        }
    }
}