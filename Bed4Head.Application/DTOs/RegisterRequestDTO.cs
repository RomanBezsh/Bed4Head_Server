using System.ComponentModel.DataAnnotations;

namespace Bed4Head.Application.DTOs
{
    public class RegisterRequestDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public string? Name { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Country { get; set; }
        public string? City { get; set; }
        public string? TravelPurpose { get; set; }

        public DateTime? Birth { get; set; }
    }
}

