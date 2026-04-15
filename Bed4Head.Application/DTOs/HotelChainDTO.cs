namespace Bed4Head.Application.DTOs
{
    public class HotelChainDTO
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public int HotelsCount { get; set; }
    }
}

