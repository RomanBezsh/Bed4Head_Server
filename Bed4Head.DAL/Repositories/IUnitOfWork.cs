using Bed4Head.DAL.Entities;

namespace Bed4Head.DAL.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Hotel> Hotels { get; }
        IRepository<Room> Rooms { get; }
        IRepository<Booking> Bookings { get; }
        IRepository<Amenity> Amenities { get; }
        IRepository<HotelChain> HotelChains { get; }
        IRepository<HotelFaq> HotelFaqs { get; }
        IRepository<HotelPhoto> HotelPhotos { get; }
        IRepository<RoomPhoto> RoomPhotos { get; }
        IRepository<Review> Reviews { get; }
        IRepository<PaymentMethod> PaymentMethods { get; }
        IRepository<NearbyPlace> NearbyPlaces { get; }

        Task<int> CompleteAsync();
    }
}