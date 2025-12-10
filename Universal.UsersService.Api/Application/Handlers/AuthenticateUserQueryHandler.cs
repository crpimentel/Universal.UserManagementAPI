using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Universal.UsersService.Api.Application.DTOs;
using Universal.UsersService.Api.Application.Queries;
using Universal.UsersService.Api.Domain.Repositories;
using Universal.UsersService.Api.Infrastructure.Security;
using BCrypt.Net;

namespace Universal.UsersService.Api.Application.Handlers
{
    /// <summary>
    /// Handler para procesar la autenticación de usuario.
    /// </summary>
    public class AuthenticateUserQueryHandler : IRequestHandler<AuthenticateUserQuery, UserAuthResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthenticateUserQueryHandler(
            IUserRepository userRepository,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<UserAuthResponse> Handle(AuthenticateUserQuery request, CancellationToken cancellationToken)
        {
            // Buscar usuario por correo
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                throw new ArgumentException("Usuario no existe.");

            // Verificar contraseña
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
                throw new ArgumentException("Credenciales inválidas.");

            // Generar JWT
            var token = _tokenService.GenerateToken(user);

            return new UserAuthResponse
            {
                Token = token
            };
        }
    }
}
