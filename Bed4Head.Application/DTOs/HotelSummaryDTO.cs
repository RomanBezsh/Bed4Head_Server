namespace Bed4Head.Application.DTOs
{
    public class HotelSummaryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int Stars { get; set; }
        public string HotelType { get; set; }
        public decimal BasePricePerNight { get; set; }
        public string CurrencyCode { get; set; }
        public double OverallRating { get; set; }
        public string? RatingLabel { get; set; }
        public int ReviewsCount { get; set; }
        public bool IsFeatured { get; set; }

        // 👇 ДОБАВЬ
        public double? DistanceFromCenterKm { get; set; }

        public List<string> Photos { get; set; } = [];
    }
    
}