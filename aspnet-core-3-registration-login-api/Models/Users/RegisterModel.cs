using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Users
{
    public class RegisterModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string EMail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}