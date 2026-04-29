using Microsoft.AspNetCore.Http;

namespace Bed4Head.Web.Models
{
    public class CreateHotelAdminRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Stars { get; set; } = 5;
        public string Type { get; set; } = "Hotel";
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
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
        public List<IFormFile> Photos { get; set; } = [];
    }
}
