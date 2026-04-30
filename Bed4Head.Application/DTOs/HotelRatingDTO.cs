namespace Bed4Head.Application.DTOs
{
    public class HotelRatingDTO
    {
        public Guid HotelId { get; set; }
        public double OverallRating { get; set; }
        public string? RatingLabel { get; set; }
        public int ReviewsCount { get; set; }
    }
}
