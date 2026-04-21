namespace Bed4Head.Domain.Entities
{
    public class Review
    {
        public Guid Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string? Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateOnly? StayedAt { get; set; }
        public string? TripType { get; set; }
        public bool IsVerifiedStay { get; set; }
        public double OverallScore { get; set; }
        public double Facilities { get; set; }
        public double Staff { get; set; }
        public double Cleanliness { get; set; }
        public double Comfort { get; set; }
        public double Location { get; set; }
        public double ValueForMoney { get; set; }
        public Guid HotelId { get; set; }
        public virtual Hotel Hotel { get; set; } = null!;
        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}