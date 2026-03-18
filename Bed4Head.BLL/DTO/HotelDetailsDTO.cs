namespace Bed4Head.BLL.DTO
{
    public class HotelDetailsDTO
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }
        public int Stars { get; set; }

        public required string Address { get; set; }
        public required string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string? Phone { get; set; }
        public string? Email { get; set; }

        public string? HotelType { get; set; } 
        public Guid? HotelChainId { get; set; }

        public double OverallRating { get; set; }
        public int ReviewsCount { get; set; }
    }
}