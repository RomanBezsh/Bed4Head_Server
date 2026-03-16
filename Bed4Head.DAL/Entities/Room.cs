using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bed4Head.DAL.Entities
{
    public class Room
    {
        public Guid Id { get; set; }
        public string Title { get; set; } 
        public decimal Price { get; set; } 

        public string BedType { get; set; } 
        public int MaxGuests { get; set; } 
        public bool FreeCancellation { get; set; } 

        public Guid HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }

        //public virtual ICollection<RoomPhoto> Photos { get; set; }
    }
}
