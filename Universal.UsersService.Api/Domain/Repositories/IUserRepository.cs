using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Universal.UsersService.Api.Domain.Entities;

namespace Universal.UsersService.Api.Domain.Repositories
{
    /// <summary>
    /// Contrato para el acceso a datos de usuarios.
    /// </summary>
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task<bool> EmailExistsAsync(string email);
    }
}
