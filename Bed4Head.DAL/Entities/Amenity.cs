
namespace Bed4Head.DAL.Entities
{
    public class Amenity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string IconKey { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
