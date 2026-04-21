namespace Bed4Head.Domain.Entities
{
    public class Room
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; } = "USD";
        public string? BedType { get; set; }
        public string? RoomType { get; set; }
        public string? View { get; set; }
        public double? AreaInSquareMeters { get; set; }
        public int MaxGuests { get; set; }
        public int AvailableUnits { get; set; }
        public bool FreeCancellation { get; set; }
        public bool BreakfastIncluded { get; set; }
        public bool PrivateBathroom { get; set; }
        public Guid HotelId { get; set; }
        public virtual Hotel? Hotel { get; set; }
        public virtual ICollection<RoomPhoto> Photos { get; set; } = new List<RoomPhoto>();
    }
}