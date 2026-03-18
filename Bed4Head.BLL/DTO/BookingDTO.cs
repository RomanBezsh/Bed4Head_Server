namespace Bed4Head.BLL.DTO
{
    public class BookingDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Pending";
        public int AdultsCount { get; set; }
        public int ChildrenCount { get; set; }
        public int TotalNights { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}