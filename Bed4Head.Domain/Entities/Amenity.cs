namespace Bed4Head.Domain.Entities
{
    public class Amenity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string IconKey { get; set; } = string.Empty;
        public bool IsHighlighted { get; set; }
        public int DisplayOrder { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}