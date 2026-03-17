using System;
using System.Collections.Generic;

namespace Bed4Head.DAL.Entities
{
    public class Hotel
    {
        public Guid Id { get; set; }

        // Main info
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stars { get; set; }

        // Address and location
        public string Address { get; set; }
        public string City { get; set; } 
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Contacts
        public string? Phone { get; set; }
        public string? Email { get; set; }

        // 
        public string HotelType { get; set; } 
        public double OverallRating { get; set; } // Например, 7.9 или 9.3 (как в кружочках)
        public int ReviewsCount { get; set; } // Количество отзывов (например, 345)


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