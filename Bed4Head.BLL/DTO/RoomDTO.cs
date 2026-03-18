namespace Bed4Head.BLL.DTO
{
    public class RoomDTO
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public decimal Price { get; set; }

        public string? BedType { get; set; }

        public int MaxGuests { get; set; }

        public bool FreeCancellation { get; set; }

        public Guid HotelId { get; set; }

        public List<string>? PhotoUrls { get; set; }
    }
}