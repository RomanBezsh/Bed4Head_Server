
namespace Bed4Head.Domain.Entities
{
    public class Booking
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public Guid RoomId { get; set; }

        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }

        public int AdultsCount { get; set; }
        public int ChildrenCount { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public bool CallMe { get; set; }
        public bool SendEmail { get; set; }
    }
}

