namespace Universal.UsersService.Api.Application.DTOs
{
    /// <summary>
    /// DTO para la creación de usuarios.
    /// </summary>
    public class UserCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para la respuesta al crear usuario.
    /// </summary>
    public class UserCreateResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid Id { get; set; }
        public string Token { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para la autenticación de usuario.
    /// </summary>
    public class UserAuthRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para la respuesta de autenticación.
    /// </summary>
    public class UserAuthResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}