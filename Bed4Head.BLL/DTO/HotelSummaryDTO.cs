namespace Bed4Head.BLL.DTO
{
    public class HotelSummaryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public int Stars { get; set; }
        public double OverallRating { get; set; }
        public int ReviewsCount { get; set; }
    }
}