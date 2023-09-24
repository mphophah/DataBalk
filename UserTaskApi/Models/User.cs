using System.ComponentModel.DataAnnotations;

namespace UserTaskApi.Controllers
{
    public class User
    {
        [Key]
        public Guid Guid { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}