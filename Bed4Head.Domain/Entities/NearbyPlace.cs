namespace Bed4Head.Domain.Entities
{
    public class NearbyPlace
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PlaceType { get; set; } = string.Empty;
        public double DistanceInMeters { get; set; }
        public Guid HotelId { get; set; }
        public virtual Hotel Hotel { get; set; } = null!;
    }
}
