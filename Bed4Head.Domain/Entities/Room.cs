
namespace Bed4Head.Domain.Entities
{
    public class Room
    {
        public Guid Id { get; set; }
        public string? Title { get; set; } 
        public decimal Price { get; set; } 

        public string? BedType { get; set; } 
        public int MaxGuests { get; set; } 
        public bool FreeCancellation { get; set; } 

        public Guid HotelId { get; set; }
        public virtual Hotel? Hotel { get; set; }

        public virtual ICollection<RoomPhoto> Photos { get; set; } = new List<RoomPhoto>();
    }
}


