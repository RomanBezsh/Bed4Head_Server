
namespace Bed4Head.DAL.Entities
{
    public class RoomPhoto
    {
        public Guid Id { get; set; }
        public required string Url { get; set; }
        public bool? IsPreview { get; set; } // Фото, которое будет на карточке выбора номера

        public Guid RoomId { get; set; }
        public virtual Room Room { get; set; } = null!;
    }
}
