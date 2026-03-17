

namespace Bed4Head.DAL.Entities
{
    public class HotelChain
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
    }
}