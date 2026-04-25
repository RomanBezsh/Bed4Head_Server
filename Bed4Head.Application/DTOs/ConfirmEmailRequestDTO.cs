using System.ComponentModel.DataAnnotations;

namespace Bed4Head.Application.DTOs
{
    public class ConfirmEmailRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Code { get; set; } = null!;
    }
}

