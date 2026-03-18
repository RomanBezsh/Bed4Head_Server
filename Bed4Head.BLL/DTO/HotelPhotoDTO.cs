namespace Bed4Head.BLL.DTO
{
    public class HotelPhotoDTO
    {
        public Guid Id { get; set; }

        public required string Url { get; set; }

        public bool IsPrimary { get; set; }

        public int DisplayOrder { get; set; }

        public Guid HotelId { get; set; }
    }
}