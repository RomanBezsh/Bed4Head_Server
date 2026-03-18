namespace Bed4Head.BLL.DTO
{
    public class HotelChainDTO
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public int HotelsCount { get; set; }
    }
}