namespace Bed4Head.Application.DTOs
{
    public class AccountSummaryDTO
    {
        public required UserDTO Profile { get; set; }
        public AccountBookingSectionsDTO Bookings { get; set; } = new();
        public AccountTravelPreferencesDTO TravelPreferences { get; set; } = new();
        public AccountNotificationPreferencesDTO NotificationPreferences { get; set; } = new();
        public List<AccountPaymentMethodDTO> PaymentMethods { get; set; } = [];
        public AccountStatsDTO Stats { get; set; } = new();
    }

    public class AccountBookingSectionsDTO
    {
        public List<BookingDTO> Upcoming { get; set; } = [];
        public List<BookingDTO> Past { get; set; } = [];
        public List<BookingDTO> Cancelled { get; set; } = [];
    }

    public class AccountTravelPreferencesDTO
    {
        public string? TravelPurpose { get; set; }
        public string? PreferredCurrencyCode { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
    }

    public class AccountNotificationPreferencesDTO
    {
        public bool NewsSeasonalOffers { get; set; }
        public bool NewsFavoriteCities { get; set; }
        public bool NewsAcrossWorld { get; set; }
        public bool NewsAffordableTravel { get; set; }
    }

    public class AccountPaymentMethodDTO
    {
        public Guid Id { get; set; }
        public required string CardType { get; set; }
        public required string LastFourDigits { get; set; }
        public string? ExpiryDate { get; set; }
        public bool IsPrimary { get; set; }
    }

    public class AccountStatsDTO
    {
        public int TotalBookings { get; set; }
        public int UpcomingBookings { get; set; }
        public int CancelledBookings { get; set; }
        public decimal TotalSpent { get; set; }
        public int ReviewsCount { get; set; }
    }
}
