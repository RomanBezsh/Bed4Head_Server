using Bed4Head.Domain.Entities;

public class RoomBed
{
    public Guid Id { get; set; }

    public string Type { get; set; } = null!; // "Queen", "Double", etc.
    public int Count { get; set; }

    public Guid RoomId { get; set; }
    public Room Room { get; set; } = null!;
}