
namespace UserTaskApi.Controllers
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<bool> UsernameOrEmailExistsAsync(string username, string email);
        Task<User> CreateUserAsync(RegisterDto registerDto);
        bool VerifyPassword(string inputPassword, string hashedPassword);
        void DeleteUserAsync(User user);
        void UpdateUserAsync(User user);
    }
}