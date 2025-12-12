using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Universal.UsersService.Api.Application.DTOs;
using Universal.UsersService.Api.Application.Handlers;
using Universal.UsersService.Api.Application.Queries;
using Universal.UsersService.Api.Domain.Entities;
using Universal.UsersService.Api.Domain.Repositories;
using Universal.UsersService.Api.Infrastructure.Security;

namespace Universal.UsersService.Tests.Handlers
{
    public class AuthenticateUserQueryHandlerTests
    {
        private const string ValidEmail = "test@example.com";
        private const string ValidPassword = "Password1!";
        private const string ValidName = "Test User";
        private const string JwtToken = "fake-jwt-token";

        [Fact]
        public async Task Handle_ShouldAuthenticateUser_WhenCredentialsAreValid()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var tokenServiceMock = new Mock<ITokenService>();
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(ValidPassword);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = ValidName,
                Email = ValidEmail,
                PasswordHash = passwordHash
            };
            userRepositoryMock.Setup(r => r.GetByEmailAsync(ValidEmail)).ReturnsAsync(user);
            tokenServiceMock.Setup(t => t.GenerateToken(user)).Returns(JwtToken);

            var handler = new AuthenticateUserQueryHandler(
                userRepositoryMock.Object,
                tokenServiceMock.Object);

            var query = new AuthenticateUserQuery
            {
                Email = ValidEmail,
                Password = ValidPassword
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(JwtToken, result.Token);
        }
    }
}
