using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Universal.UsersService.Api.Application.Commands;
using Universal.UsersService.Api.Application.DTOs;
using Universal.UsersService.Api.Domain.Entities;
using Universal.UsersService.Api.Domain.Repositories;
using Universal.UsersService.Api.Infrastructure.Security;
using BCrypt.Net;

namespace Universal.UsersService.Api.Application.Handlers
{
    /// <summary>
    /// Handler para procesar el registro de usuario.
    /// </summary>
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserCreateResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IConfiguration configuration,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public async Task<UserCreateResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // Validación de nombre
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("El nombre no puede estar vacío.");

            // Obtener expresiones regulares desde configuración
            var emailRegex = _configuration["Validation:EmailRegex"] ?? "";
            var passwordRegex = _configuration["Validation:PasswordRegex"] ?? "";

            // Validación de correo
            if (string.IsNullOrWhiteSpace(emailRegex) || !System.Text.RegularExpressions.Regex.IsMatch(request.Email, emailRegex))
                throw new ArgumentException("El correo no tiene un formato válido.");

            // Validación de contraseña
            if (string.IsNullOrWhiteSpace(passwordRegex) || !System.Text.RegularExpressions.Regex.IsMatch(request.Password, passwordRegex))
                throw new ArgumentException("La contraseña debe tener números, mayúsculas, minúsculas, símbolos y más de 8 caracteres.");

            // Validación de unicidad de correo
            if (await _userRepository.EmailExistsAsync(request.Email))
                throw new ArgumentException("El correo ya se encuentra registrado.");

            // Encriptar contraseña
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Crear usuario
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                PasswordHash = passwordHash
            };

            await _userRepository.AddAsync(user);

            // Generar JWT para el usuario registrado
            var token = _tokenService.GenerateToken(user);

            return new UserCreateResponse
            {
                Name = user.Name,
                Email = user.Email,
                Id = user.Id,
                Token = token
            };
        }
    }
}
