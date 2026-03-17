
namespace Bed4Head.DAL.Entities
{
    public class Review
    {
        public Guid Id { get; set; }
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; }
        public double Facilities { get; set; }
        public double Staff { get; set; }
        public double Cleanliness { get; set; }
        public double Comfort { get; set; }
        public double Location { get; set; }
        public double ValueForMoney { get; set; }

        public Guid HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
