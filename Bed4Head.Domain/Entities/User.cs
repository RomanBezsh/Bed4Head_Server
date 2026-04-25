namespace Bed4Head.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public string? DisplayName { get; set; }
        public string? Phone { get; set; }
        public DateTime? BirthDate { get; set; }

        public string? Country { get; set; }
        public string? City { get; set; }

        public bool? IsEmailConfirmed { get; set; }

        public string? TravelPurpose { get; set; } 
        public string? PreferredCurrencyCode { get; set; }
        public string? AvatarUrl { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool? NewsSeasonalOffers { get; set; }
        public bool? NewsFavoriteCities { get; set; }
        public bool? NewsAcrossWorld { get; set; }
        public bool? NewsAffordableTravel { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        public virtual ICollection<Hotel> FavoriteHotels { get; set; } = new List<Hotel>();

        public virtual ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();

        public virtual ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
    }
}

