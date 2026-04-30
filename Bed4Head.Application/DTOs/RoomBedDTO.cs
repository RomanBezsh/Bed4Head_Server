public class RoomBedDTO
{
    public Guid Id { get; set; }

    public string Type { get; set; } = null!; // "queen-size", "double"

    public int Count { get; set; }

    public Guid RoomId { get; set; }
}