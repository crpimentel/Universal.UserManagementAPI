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

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserCreateResponse> RegisterUserAsync(UserCreateRequest request)
        {
            // Aquí se implementarán las validaciones y lógica de registro
            // (Se completará en pasos siguientes)
            throw new NotImplementedException();
        }

        public async Task<UserAuthResponse> AuthenticateUserAsync(UserAuthRequest request)
        {
            // Aquí se implementará la lógica de autenticación
            // (Se completará en pasos siguientes)
            throw new NotImplementedException();
        }
    }
}