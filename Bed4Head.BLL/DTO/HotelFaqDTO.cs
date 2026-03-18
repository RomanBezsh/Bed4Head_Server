namespace Bed4Head.BLL.DTO
{
    public class HotelFaqDTO
    {
        public Guid Id { get; set; }

        public required string Question { get; set; }

        public required string Answer { get; set; }

        public int DisplayOrder { get; set; }

        public Guid HotelId { get; set; }
    }
}