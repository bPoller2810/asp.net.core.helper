using System.ComponentModel.DataAnnotations;

namespace helper.sample.Database
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public User()
        {
            Username = string.Empty;
            PasswordHash = string.Empty;
        }
    }
}