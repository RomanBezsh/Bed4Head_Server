using System;
using System.Collections.Generic;

namespace Bed4Head.DAL.Entities
{
    public class Hotel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Stars { get; set; } 

        public string Address { get; set; }
        public double Latitude { get; set; }  
        public double Longitude { get; set; } 

        public string? Phone { get; set; }

        //public virtual ICollection<HotelPhoto> Photos { get; set; } = new List<HotelPhoto>();

        //public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

        //public virtual ICollection<HotelAmenity> Amenities { get; set; } = new List<HotelAmenity>();

        //public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        //public virtual ICollection<HotelFaq> Faqs { get; set; } = new List<HotelFaq>();
    }
}