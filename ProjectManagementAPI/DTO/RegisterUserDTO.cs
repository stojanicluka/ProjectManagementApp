using System.ComponentModel.DataAnnotations;

namespace ProjectManagementAPI.DTO
{
    public class RegisterUserDTO
    {
        [Required]
        public String FirstName { get; set; }

        [Required]
        public String LastName { get; set; }

        [Required]
        public String Username { get; set; }

        [Required]
        public String Email { get; set; }

        [Required]
        public String Password { get; set; }
    }
}
