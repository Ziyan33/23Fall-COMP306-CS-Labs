using System.ComponentModel.DataAnnotations;

namespace _301133339_liu_lab3.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
