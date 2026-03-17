using Bed4Head.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bed4Head.DAL.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<HotelChain> HotelChains { get; set; }
        public DbSet<HotelPhoto> HotelPhotos { get; set; }
        public DbSet<HotelFaq> HotelFaqs { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<NearbyPlace> NearbyPlaces { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomPhoto> RoomPhotos { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Amenity> Amenities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) // Проверь это имя
        {
            base.OnModelCreating(modelBuilder);

            // Настройка UUID для PostgreSQL
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id)
                      .HasDefaultValueSql("gen_random_uuid()");
            });

            // Если ты переименовал HotelRatings в Reviews, 
            // проверь, чтобы в миграциях не осталось старых ссылок.
        }
    }
}