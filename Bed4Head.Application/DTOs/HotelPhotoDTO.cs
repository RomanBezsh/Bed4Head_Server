namespace Bed4Head.Application.DTOs
{
    public class HotelPhotoDTO
    {
        public Guid Id { get; set; }
        public required string Url { get; set; }
        public string? Caption { get; set; }
        public string? Category { get; set; }
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
        public Guid HotelId { get; set; }
    }
}