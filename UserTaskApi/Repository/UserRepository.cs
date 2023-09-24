using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace UserTaskApi.Controllers
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public UserRepository(UserDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> UsernameOrEmailExistsAsync(string username, string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Username == username || u.Email == email);
        }

        public async Task<User> CreateUserAsync(RegisterDto registerDto)
        {
            var hashedPassword = HashPassword(registerDto.Password);

            var user = new User
            {
                Guid = Guid.NewGuid(),
                Username = registerDto.Username,
                Email = registerDto.Email,
                Password = hashedPassword
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            string hashedInput = HashPassword(inputPassword);
            return string.Equals(hashedInput, hashedPassword);
        }

        private string HashPassword(string password)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(_configuration["Token:Key"]);
            using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedBytes = hmac.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public void DeleteUserAsync(User user)
        {
            _dbContext.Users.Remove(user);
             _dbContext.SaveChangesAsync();
        }
        public void UpdateUserAsync(User user)
        {
            // Update user information in the database.
             _dbContext.SaveChangesAsync();
        }
    }
}