using System.ComponentModel.DataAnnotations;

namespace Bed4Head.Application.DTOs
{
    public class UpdateProfileRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Country { get; set; } = null!;

        [Required]
        public string City { get; set; } = null!;

        [Required]
        public string TravelPurpose { get; set; } = null!;

        public string? PreferredCurrencyCode { get; set; }

        public List<Guid>? TravelAmenityIds { get; set; }
    }
}

