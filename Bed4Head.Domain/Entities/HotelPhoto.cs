namespace Bed4Head.Domain.Entities
{
    public class HotelPhoto
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? Caption { get; set; }
        public string? Category { get; set; }
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
        public Guid HotelId { get; set; }
        public virtual Hotel Hotel { get; set; } = null!;
    }
}