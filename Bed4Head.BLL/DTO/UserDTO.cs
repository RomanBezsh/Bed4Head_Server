namespace Bed4Head.BLL.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        public required string Email { get; set; }

        public string? DisplayName { get; set; }
        public string? Phone { get; set; }
        public DateTime? BirthDate { get; set; }

        public string? Country { get; set; }
        public string? City { get; set; }

        public bool IsEmailConfirmed { get; set; }
        public string? AvatarUrl { get; set; }
        public string? TravelPurpose { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool NewsSeasonalOffers { get; set; }
        public bool NewsFavoriteCities { get; set; }
        public bool NewsAcrossWorld { get; set; }
        public bool NewsAffordableTravel { get; set; }
    }
}