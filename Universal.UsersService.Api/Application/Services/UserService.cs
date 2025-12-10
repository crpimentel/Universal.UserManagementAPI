using System;
using System.Threading.Tasks;
using Universal.UsersService.Api.Application.DTOs;
using Universal.UsersService.Api.Domain.Entities;
using Universal.UsersService.Api.Domain.Repositories;
using BCrypt.Net;

namespace Universal.UsersService.Api.Application.Services
{
    /// <summary>
    /// Implementación del servicio de aplicación para gestión de usuarios.
    /// </summary>
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly Universal.UsersService.Api.Infrastructure.Security.ITokenService _tokenService;

        public UserService(
            IUserRepository userRepository,
            IConfiguration configuration,
            Universal.UsersService.Api.Infrastructure.Security.ITokenService tokenService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _tokenService = tokenService;
        }


        public async Task<UserCreateResponse> RegisterUserAsync(UserCreateRequest request)
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
                throw new ArgumentException("La contraseña debe tener mayúsculas, minúsculas, símbolos y más de 8 caracteres.");

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

        public async Task<UserAuthResponse> AuthenticateUserAsync(UserAuthRequest request)
        {
            // Aquí se implementará la lógica de autenticación
            // (Se completará en pasos siguientes)
            throw new NotImplementedException();
        }
    }
}