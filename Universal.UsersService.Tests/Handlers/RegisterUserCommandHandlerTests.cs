using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Microsoft.Extensions.Configuration;
using Universal.UsersService.Api.Application.Commands;
using Universal.UsersService.Api.Application.DTOs;
using Universal.UsersService.Api.Application.Handlers;
using Universal.UsersService.Api.Domain.Entities;
using Universal.UsersService.Api.Domain.Repositories;
using Universal.UsersService.Api.Infrastructure.Security;

namespace Universal.UsersService.Tests.Handlers
{
    public class RegisterUserCommandHandlerTests
    {
        private const string ValidEmail = "test@example.com";
        private const string ValidPassword = "P@ssword1!";
        private const string ValidName = "Test User";
        private const string JwtToken = "fake-jwt-token";
        private const string EmailRegex = "^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$";
        private const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+={\}:;<>?,.-]).{9,}$";


        [Fact]
        public async Task Handle_ShouldRegisterUser_WhenDataIsValid()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var tokenServiceMock = new Mock<ITokenService>();
            var configurationMock = new Mock<IConfiguration>();

            configurationMock.Setup(c => c["Validation:EmailRegex"]).Returns(EmailRegex);
            configurationMock.Setup(c => c["Validation:PasswordRegex"]).Returns(PasswordRegex);
            userRepositoryMock.Setup(r => r.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            tokenServiceMock.Setup(t => t.GenerateToken(It.IsAny<User>())).Returns(JwtToken);

            var handler = new RegisterUserCommandHandler(
                userRepositoryMock.Object,
                configurationMock.Object,
                tokenServiceMock.Object);

            var command = new RegisterUserCommand
            {
                Name = ValidName,
                Email = ValidEmail,
                Password = ValidPassword
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ValidName, result.Name);
            Assert.Equal(ValidEmail, result.Email);
            Assert.False(result.Id == Guid.Empty);
            Assert.Equal(JwtToken, result.Token);
        }
    }
}
