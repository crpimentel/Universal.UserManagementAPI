using System;

namespace Universal.UsersService.Api.Domain.Entities
{
    /// <summary>
    /// Entidad principal para usuarios.
    /// </summary>
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
