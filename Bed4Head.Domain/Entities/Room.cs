namespace Bed4Head.Domain.Entities
{
    public class Room
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public decimal Price { get; set; }

        public string CurrencyCode { get; set; } = "USD";

        public int MaxGuests { get; set; }

        public bool FreeCancellation { get; set; }

        public bool PrivateBathroom { get; set; }

        public bool HasWifi { get; set; }

        public bool HasPrivatePool { get; set; }

        public Guid HotelId { get; set; }

        public virtual ICollection<RoomPhoto> Photos { get; set; } = new List<RoomPhoto>();

        public virtual ICollection<RoomBed> Beds { get; set; } = new List<RoomBed>();
    }
}