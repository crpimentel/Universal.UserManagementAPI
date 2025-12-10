using System;
using System.Threading.Tasks;
using Universal.UsersService.Api.Application.DTOs;

namespace Universal.UsersService.Api.Application.Services
{
    /// <summary>
    /// Servicio de aplicación para gestión de usuarios.
    /// </summary>
    public interface IUserService
    {
        Task<UserCreateResponse> RegisterUserAsync(UserCreateRequest request);
        Task<UserAuthResponse> AuthenticateUserAsync(UserAuthRequest request);
    }
}