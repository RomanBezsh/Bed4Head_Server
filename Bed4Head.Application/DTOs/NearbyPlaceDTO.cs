namespace Bed4Head.Application.DTOs
{
    public class NearbyPlaceDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string PlaceType { get; set; }
        public double DistanceInMeters { get; set; }
        public Guid HotelId { get; set; }
    }
}
