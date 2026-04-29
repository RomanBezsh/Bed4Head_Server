using Microsoft.AspNetCore.Http;

namespace Bed4Head.Application.DTOs
{
    public class CreateRoomRequestDTO
    {
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; } = "USD";
        public int MaxGuests { get; set; }
        public Guid HotelId { get; set; }

        public bool FreeCancellation { get; set; }
        public bool PrivateBathroom { get; set; }
        public bool HasWifi { get; set; }
        public bool HasPrivatePool { get; set; }

        public string? Beds { get; set; } // JSON

        public IFormFile? PreviewImage { get; set; } // ✅ ВАЖНО
    }
}