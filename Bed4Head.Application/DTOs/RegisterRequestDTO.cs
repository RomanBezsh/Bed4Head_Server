using System.ComponentModel.DataAnnotations;

namespace Bed4Head.Application.DTOs
{
    public class RegisterRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}

