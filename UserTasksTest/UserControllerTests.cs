using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserTaskApi.Controllers;
using UserTaskApi.Data;
using Xunit;
using System.Threading.Tasks;
using UserTaskApi.Services;

public class UserControllerTests
{
    private readonly UserController _controller;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IConfiguration> _configuration;

    public UserControllerTests()
    {
        // Arrange
        _userRepository = new Mock<IUserRepository>();
        _configuration = new Mock<IConfiguration>();
        var jwtService = new JwtService(_configuration.Object); // Create an instance of JwtService
        _controller = new UserController(_configuration.Object, _userRepository.Object, jwtService);
    }

    [Fact]
    public async System.Threading.Tasks.Task Register_WithValidDto_ReturnsOkResult()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Username = "testuser",
            Email = "testuser@example.com",
            Password = "password"
        };

        _userRepository.Setup(repo => repo.UsernameOrEmailExistsAsync(registerDto.Username, registerDto.Email))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.Register(registerDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async System.Threading.Tasks.Task Register_WithExistingUsernameOrEmail_ReturnsBadRequest()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Username = "existinguser",
            Email = "existinguser@example.com",
            Password = "password"
        };

        _userRepository.Setup(repo => repo.UsernameOrEmailExistsAsync(registerDto.Username, registerDto.Email))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Register(registerDto) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public async System.Threading.Tasks.Task Login_WithValidCredentials_ReturnsOkResult()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Username = "testuser",
            Password = "password"
        };

        var user = new User
        {
            Username = loginDto.Username,
            Password = "$2a$12$CgFI6qZT/sD/FvUOn3GlpO87uV2r9BLHR6RWfi.F4BSCl8HzsR2y6\r\n"
        };

        _userRepository.Setup(repo => repo.GetUserByUsernameAsync(loginDto.Username))
            .ReturnsAsync(user);
        _userRepository.Setup(repo => repo.VerifyPassword(loginDto.Password, user.Password))
            .Returns(true);

        // Mock the token generation method if necessary
        _configuration.Setup(cfg => cfg["Token:Key"]).Returns("YourSecretKey");
        _configuration.Setup(cfg => cfg["Token:Issuer"]).Returns("YourIssuer");

        // Act
        var result = await _controller.Login(loginDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async System.Threading.Tasks.Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Username = "testuser",
            Password = "wrong_password"
        };

        var user = new User
        {
            Username = loginDto.Username,
            Password = "$2a$12$CgFI6qZT/sD/FvUOn3GlpO87uV2r9BLHR6RWfi.F4BSCl8HzsR2y6\r\n"
        };

        _userRepository.Setup(repo => repo.GetUserByUsernameAsync(loginDto.Username))
            .ReturnsAsync(user);
        _userRepository.Setup(repo => repo.VerifyPassword(loginDto.Password, user.Password))
            .Returns(false);

        // Act
        var result = await _controller.Login(loginDto) as UnauthorizedObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(401, result.StatusCode);
    }

    //****************************************************************
    [Fact]
    public async System.Threading.Tasks.Task Delete_WithValidUsername_ReturnsOkResult()
    {
        // Arrange
        var username = "testuser";

        var user = new User
        {
            Username = username,
            Email = "testuser@example.com",
            Password = "password"
        };

        _userRepository.Setup(repo => repo.GetUserByUsernameAsync(username))
            .ReturnsAsync(user);

        // Act
        var result = await _controller.Delete(username) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async System.Threading.Tasks.Task Delete_WithNonExistentUsername_ReturnsNotFound()
    {
        // Arrange
        var username = "nonexistentuser";

        _userRepository.Setup(repo => repo.GetUserByUsernameAsync(username))
            .ReturnsAsync((User)null);

        // Act
        var result = await _controller.Delete(username) as NotFoundObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async System.Threading.Tasks.Task Update_WithValidUsernameAndDto_ReturnsOkResult()
    {
        // Arrange
        var username = "testuser";
        var updateUserDto = new UpdateUserDto
        {
            Username = "newusername",
            Email = "newemail@example.com",
            Password = "newpassword"
        };

        var user = new User
        {
            Username = username,
            Email = "testuser@example.com",
            Password = "password"
        };

        _userRepository.Setup(repo => repo.GetUserByUsernameAsync(username))
            .ReturnsAsync(user);

        // Act
        var result = await _controller.Update(username, updateUserDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async System.Threading.Tasks.Task Update_WithNonExistentUsername_ReturnsNotFound()
    {
        // Arrange
        var username = "nonexistentuser";
        var updateUserDto = new UpdateUserDto
        {
            Username = "newusername",
            Email = "newemail@example.com",
            Password = "newpassword"
        };

        _userRepository.Setup(repo => repo.GetUserByUsernameAsync(username))
            .ReturnsAsync((User)null);

        // Act
        var result = await _controller.Update(username, updateUserDto) as NotFoundObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }

}
