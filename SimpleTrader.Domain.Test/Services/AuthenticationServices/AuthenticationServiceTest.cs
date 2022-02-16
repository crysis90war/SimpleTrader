using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using SimpleTrader.Domain.Exceptions;
using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services;
using SimpleTrader.Domain.Services.AuthenticationServices;
using System.Threading.Tasks;

namespace SimpleTrader.Domain.Test.Services.AuthenticationServices
{
    [TestFixture]
    public class AuthenticationServiceTest
    {
        private Mock<IAccountService> _mockAccountService;
        private Mock<IPasswordHasher> _mockPasswordHasher;
        private AuthenticationService _authenticationService;

        [SetUp]
        public void SetUp()
        {
            _mockAccountService = new Mock<IAccountService>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _authenticationService = new AuthenticationService(
                _mockAccountService.Object,
                _mockPasswordHasher.Object);

        }

        [Test]
        public async Task Login_WithCorrectPasswordForExistingUsername_ReturnsAccountForCorrectUsername()
        {
            // Arrange
            string expectedUsername = "testuser";
            string password = "testpassword";

            _mockAccountService.Setup(s => s.GetByUsername(expectedUsername)).ReturnsAsync(
                new Account()
                {
                    AccountHolder = new User()
                    {
                        Username = expectedUsername
                    }
                });

            _mockPasswordHasher.Setup(s => s.VerifyHashedPassword(It.IsAny<string>(), password))
                .Returns(PasswordVerificationResult.Success);

            // Act
            Account account = await _authenticationService.Login(expectedUsername, password);

            // Assert
            string actualUsername = account.AccountHolder.Username;
            Assert.AreEqual(expectedUsername, actualUsername);
        }

        [Test]
        public void Login_WithIncorrectPasswordForExistingUsername_ThrowInvalidPasswordExceptionForUsername()
        {
            // Arrange
            string expectedUsername = "testuser";
            string password = "testpassword";

            _mockAccountService.Setup(s => s.GetByUsername(expectedUsername)).ReturnsAsync(
                new Account()
                {
                    AccountHolder = new User()
                    {
                        Username = expectedUsername
                    }
                });

            _mockPasswordHasher.Setup(s => s.VerifyHashedPassword(It.IsAny<string>(), password))
                .Returns(PasswordVerificationResult.Failed);

            // Act
            InvalidPasswordException exception = Assert.ThrowsAsync<InvalidPasswordException>(
                () => _authenticationService.Login(expectedUsername, password));

            // Assert
            string actualUsername = exception.Username;
            Assert.AreEqual(expectedUsername, actualUsername);
        }

        [Test]
        public void Login_NonExistingUsername_ThrowInvalidPasswordExceptionForUsername()
        {
            // Arrange
            string expectedUsername = "testuser";
            string password = "testpassword";

            _mockPasswordHasher.Setup(s => s.VerifyHashedPassword(It.IsAny<string>(), password))
                .Returns(PasswordVerificationResult.Failed);

            // Act
            UserNotFoundException exception = Assert.ThrowsAsync<UserNotFoundException>(
                () => _authenticationService.Login(expectedUsername, password));

            // Assert
            string actualUsername = exception.Username;
            Assert.AreEqual(expectedUsername, actualUsername);
        }

        [Test]
        public async Task Register_WithPasswordsNotMatching_ReturnsPasswordDoNotMatch()
        {
            // Arrange
            string password = "testpassword";
            string confirmPassword = "confirmtestpassword";
            RegistrationResult expected = RegistrationResult.PasswordsDoNotMatch;

            // Act
            RegistrationResult actual = await _authenticationService.Register(
                It.IsAny<string>(),
                It.IsAny<string>(),
                password,
                confirmPassword);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Register_WithAlreadtExistingEmail_ReturnsEmailAlreadyExists()
        {
            // Arrange
            string email = "test@gmail.com";
            _mockAccountService.Setup(s => s.GetByEmail(email))
                .ReturnsAsync(new Account());

            RegistrationResult expected = RegistrationResult.EmailAlreadyExists;

            // Act
            RegistrationResult actual = await _authenticationService.Register(
                email,
                It.IsAny<string>(),
                "test",
                "test");

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Register_WithAlreadtExistingUsername_ReturnsUsernameAlreadyExists()
        {
            // Arrange
            string username = "testuser";
            _mockAccountService.Setup(s => s.GetByUsername(username)).ReturnsAsync(new Account());
            RegistrationResult expected = RegistrationResult.UsernameAlreadyExists;

            // Act
            RegistrationResult actual = await _authenticationService.Register(
                It.IsAny<string>(),
                username,
                "test",
                "test");

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Register_WithNonExistingUserAndMatchingPasswords_ReturnsSuccess()
        {
            // Arrange
            RegistrationResult expected = RegistrationResult.Success;

            // Act
            RegistrationResult actual = await _authenticationService.Register(
                It.IsAny<string>(),
                It.IsAny<string>(),
                "test",
                "test");

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
