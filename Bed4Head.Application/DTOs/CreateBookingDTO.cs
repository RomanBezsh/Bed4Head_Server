namespace Bed4Head.Application.DTOs;

public class CreateBookingDTO
{
    public Guid RoomId { get; set; }

    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }

    public int AdultsCount { get; set; }
    public int ChildrenCount { get; set; }

    public bool CallMe { get; set; }
    public bool SendEmail { get; set; }
}