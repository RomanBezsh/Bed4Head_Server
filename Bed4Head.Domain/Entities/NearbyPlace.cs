namespace Bed4Head.Domain.Entities
{
    public class NearbyPlace
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PlaceType { get; set; } = string.Empty;
        public string? Address { get; set; }
        public double DistanceInMeters { get; set; }
        public int? WalkingMinutes { get; set; }
        public Guid HotelId { get; set; }
        public virtual Hotel Hotel { get; set; } = null!;
    }
}
