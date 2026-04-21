using System;
using System.Collections.Generic;
namespace Bed4Head.Domain.Entities
{
    public class Hotel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }
        public int Stars { get; set; }
        public string HotelType { get; set; } = "Hotel";
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? PostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? DistanceFromCenterKm { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? WebsiteUrl { get; set; }
        public decimal BasePricePerNight { get; set; }
        public string CurrencyCode { get; set; } = "USD";
        public double OverallRating { get; set; }
        public string? RatingLabel { get; set; }
        public int ReviewsCount { get; set; }
        public TimeOnly? CheckInFrom { get; set; }
        public TimeOnly? CheckOutUntil { get; set; }
        public bool PetsAllowed { get; set; }
        public bool HasFreeWifi { get; set; }
        public bool HasParking { get; set; }
        public bool IsFeatured { get; set; }
        public Guid? HotelChainId { get; set; }
        public virtual HotelChain? HotelChain { get; set; }
        public virtual ICollection<HotelPhoto> Photos { get; set; } = new List<HotelPhoto>();
        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
        public virtual ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<HotelFaq> Faqs { get; set; } = new List<HotelFaq>();
        public virtual ICollection<NearbyPlace> NearbyPlaces { get; set; } = new List<NearbyPlace>();
    }
}