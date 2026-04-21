namespace Bed4Head.Application.DTOs
{
    public class HotelSummaryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = string.Empty;
        public int Stars { get; set; }
        public string? HotelType { get; set; }
        public decimal BasePricePerNight { get; set; }
        public string CurrencyCode { get; set; } = "USD";
        public double OverallRating { get; set; }
        public string? RatingLabel { get; set; }
        public int ReviewsCount { get; set; }
        public bool IsFeatured { get; set; }
    }
}