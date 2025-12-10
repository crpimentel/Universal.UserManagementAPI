using MediatR;
using Universal.UsersService.Api.Application.DTOs;

namespace Universal.UsersService.Api.Application.Queries
{
    /// <summary>
    /// Query para autenticar un usuario.
    /// </summary>
    public class AuthenticateUserQuery : IRequest<UserAuthResponse>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
