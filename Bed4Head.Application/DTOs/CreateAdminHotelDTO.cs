namespace Bed4Head.Application.DTOs
{
    public class CreateAdminHotelDTO
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int Stars { get; set; } = 5;
        public string HotelType { get; set; } = "Hotel";
        public required string Address { get; set; }
        public required string City { get; set; }
        public string Country { get; set; } = string.Empty;
        public string? PostalCode { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public decimal BasePricePerNight { get; set; }
        public string CurrencyCode { get; set; } = "USD";
        public string? Coordinates { get; set; }
        public double? DistanceFromCenterKm { get; set; }
        public string? NearbyPlaces { get; set; }
        public string? ImportantInfo { get; set; }
        public string? Status { get; set; }
        public List<string> Facilities { get; set; } = [];
        public List<string> Faqs { get; set; } = [];
        public List<string> PhotoUrls { get; set; } = [];
    }
}
