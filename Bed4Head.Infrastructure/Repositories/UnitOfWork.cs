using Bed4Head.Infrastructure.Data;
using Bed4Head.Domain.Entities;

namespace Bed4Head.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        private IRepository<User>? _userRepository;
        private IRepository<Hotel>? _hotelRepository;
        private IRepository<Room>? _roomRepository;
        private IRepository<Booking>? _bookingRepository;
        private IRepository<Amenity>? _amenityRepository;
        private IRepository<HotelChain>? _hotelChainRepository;
        private IRepository<HotelFaq>? _hotelFaqRepository;
        private IRepository<HotelPhoto>? _hotelPhotoRepository;
        private IRepository<RoomPhoto>? _roomPhotoRepository;
        private IRepository<RoomBed>? _roomBedRepository;
        private IRepository<Review>? _reviewRepository;
        private IRepository<PaymentMethod>? _paymentMethodRepository;
        private IRepository<NearbyPlace>? _nearbyPlaceRepository;

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
        }

        public IRepository<User> Users => _userRepository ??= new UserRepository(_db);
        public IRepository<Hotel> Hotels => _hotelRepository ??= new HotelRepository(_db);
        public IRepository<Room> Rooms => _roomRepository ??= new RoomRepository(_db);
        public IRepository<Booking> Bookings => _bookingRepository ??= new BookingRepository(_db);
        public IRepository<Amenity> Amenities => _amenityRepository ??= new AmenityRepository(_db);
        public IRepository<HotelChain> HotelChains => _hotelChainRepository ??= new HotelChainRepository(_db);
        public IRepository<HotelFaq> HotelFaqs => _hotelFaqRepository ??= new HotelFaqRepository(_db);
        public IRepository<HotelPhoto> HotelPhotos => _hotelPhotoRepository ??= new HotelPhotoRepository(_db);
        public IRepository<RoomPhoto> RoomPhotos => _roomPhotoRepository ??= new RoomPhotoRepository(_db);
        public IRepository<Review> Reviews => _reviewRepository ??= new ReviewRepository(_db);
        public IRepository<RoomBed> RoomBeds => _roomBedRepository ??= new RoomBedRepository(_db);
        public IRepository<PaymentMethod> PaymentMethods => _paymentMethodRepository ??= new PaymentMethodRepository(_db);
        public IRepository<NearbyPlace> NearbyPlaces => _nearbyPlaceRepository ??= new NearbyPlaceRepository(_db);

        public async Task<int> CompleteAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

