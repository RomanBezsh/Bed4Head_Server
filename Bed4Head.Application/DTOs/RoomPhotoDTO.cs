namespace Bed4Head.Application.DTOs
{
    public class RoomPhotoDTO
    {
        public Guid Id { get; set; }

        public required string Url { get; set; }

        public bool IsPreview { get; set; }

        public Guid RoomId { get; set; }
    }
}

