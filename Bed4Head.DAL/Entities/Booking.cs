
namespace Bed4Head.DAL.Entities
{
    public class Booking
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public Guid RoomId { get; set; }
        public virtual Room Room { get; set; } = null!;

        public DateTime CheckIn { get; set; }  
        public DateTime CheckOut { get; set; } 

        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled, Completed

        public int AdultsCount { get; set; }
        public int ChildrenCount { get; set; }

        public int TotalNights => (CheckOut.Date - CheckIn.Date).Days;
          
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}