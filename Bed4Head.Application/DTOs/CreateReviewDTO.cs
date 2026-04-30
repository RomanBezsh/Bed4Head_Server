namespace Bed4Head.Application.DTOs
{
    public class CreateReviewDTO
    {
        public string? Comment { get; set; }
        public string? Title { get; set; }
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
        public Guid UserId { get; set; }
    }
}
