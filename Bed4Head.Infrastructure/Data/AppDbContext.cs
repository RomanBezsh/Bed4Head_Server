using Bed4Head.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bed4Head.Infrastructure.Data
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
        public DbSet<RoomBed> RoomBeds { get; set; }
        public DbSet<RoomPhoto> RoomPhotos { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Amenity> Amenities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) // ������� ��� ���
        {
            base.OnModelCreating(modelBuilder);

            // ��������� UUID ��� PostgreSQL
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id)
                      .HasDefaultValueSql("gen_random_uuid()");
            });

            // ���� �� ������������ HotelRatings � Reviews, 
            // �������, ����� � ��������� �� �������� ������ ������.
        }
    }
}

