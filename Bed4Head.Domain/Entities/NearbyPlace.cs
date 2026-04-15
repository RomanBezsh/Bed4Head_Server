
namespace Bed4Head.Domain.Entities
{
    public class NearbyPlace
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }     
        public required string PlaceType { get; set; } 

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double DistanceInMeters { get; set; }

        public Guid HotelId { get; set; }
        public virtual Hotel Hotel { get; set; } = null!;
    }
}

