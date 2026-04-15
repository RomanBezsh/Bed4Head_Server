namespace Bed4Head.Application.DTOs
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

