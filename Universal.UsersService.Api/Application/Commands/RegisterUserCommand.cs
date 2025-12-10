using MediatR;
using Universal.UsersService.Api.Application.DTOs;

namespace Universal.UsersService.Api.Application.Commands
{
    /// <summary>
    /// Command para registrar un nuevo usuario.
    /// </summary>
    public class RegisterUserCommand : IRequest<UserCreateResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
