namespace Bed4Head.Application.DTOs
{
    public class HotelDetailsDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int Stars { get; set; }
        public string? HotelType { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public string Country { get; set; } = string.Empty;
        public string? PostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? DistanceFromCenterKm { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public decimal BasePricePerNight { get; set; }
        public string CurrencyCode { get; set; } = "USD";
        public List<ImportantInfoItemDTO> ImportantInfo { get; set; } = [];
        public double OverallRating { get; set; }
        public string? RatingLabel { get; set; }
        public int ReviewsCount { get; set; }
        public bool IsFeatured { get; set; }
        public Guid? HotelChainId { get; set; }
    }
}
