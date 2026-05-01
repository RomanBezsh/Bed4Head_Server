using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Bed4Head.Domain.Entities;
using Bed4Head.Infrastructure.Data;
using Bed4Head.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bed4Head.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _db;
        private readonly AppDbContext _context;

        public BookingService(IUnitOfWork db, AppDbContext context)
        {
            _db = db;
            _context = context;
        }

        public async Task<IEnumerable<BookingDTO>> GetAllAsync()
        {
            var rows = await BuildBookingQuery(_context.Bookings
                    .AsNoTracking()
                    .OrderByDescending(b => b.CreatedAt))
                .ToListAsync();

            return rows.Select(x => MapBookingDto(x.Booking, x.Room, x.Hotel));
        }

        public async Task<BookingDTO?> GetByIdAsync(Guid id)
        {
            var booking = await BuildBookingQuery(_context.Bookings
                    .AsNoTracking()
                    .Where(b => b.Id == id))
                .FirstOrDefaultAsync();

            return booking == null
                ? null
                : MapBookingDto(booking.Booking, booking.Room, booking.Hotel);
        }

        public async Task<IEnumerable<BookingDTO>> GetByUserIdAsync(Guid userId)
        {
            var rows = await BuildBookingQuery(_context.Bookings
                    .AsNoTracking()
                    .Where(b => b.UserId == userId)
                    .OrderByDescending(b => b.CreatedAt))
                .ToListAsync();

            return rows.Select(x => MapBookingDto(x.Booking, x.Room, x.Hotel));
        }

        public async Task<BookingDTO> CreateAsync(CreateBookingDTO dto, Guid userId)
        {
            var existingBookingExists = await _context.Bookings
                .AsNoTracking()
                .AnyAsync(b => b.RoomId == dto.RoomId &&
                               (b.Status == null || b.Status.ToLower() != "cancelled") &&
                               dto.CheckIn < b.CheckOut &&
                               dto.CheckOut > b.CheckIn);

            if (existingBookingExists)
                throw new Exception("Room already booked for selected dates");

            var room = await _db.Rooms.GetByIdAsync(dto.RoomId);
            if (room == null)
                throw new Exception("Room not found");

            if (dto.CheckOut <= dto.CheckIn)
                throw new Exception("Invalid dates");

            var days = (dto.CheckOut.Date - dto.CheckIn.Date).Days;
            var totalPrice = room.Price * days;

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut,
                Status = "Pending", // теперь сервер сам ставит
                UserId = userId,    // 🔥 берем из JWT, не из клиента
                RoomId = dto.RoomId,
                AdultsCount = dto.AdultsCount,
                ChildrenCount = dto.ChildrenCount,
                TotalPrice = totalPrice,
                CreatedAt = DateTime.UtcNow,

                CallMe = dto.CallMe,
                SendEmail = dto.SendEmail
            };

            await _db.Bookings.AddAsync(booking);
            await _db.CompleteAsync();

            return await GetByIdAsync(booking.Id) ?? MapBookingDto(booking, room, null);
        }
        
        public async Task UpdateStatusAsync(Guid id, string status)
        {
            var booking = await _db.Bookings.GetByIdAsync(id);
            if (booking != null)
            {
                booking.Status = status;
                await _db.Bookings.UpdateAsync(booking);
                await _db.CompleteAsync();
            }
        }

        public async Task CancelAsync(Guid id)
        {
            var booking = await _db.Bookings.GetByIdAsync(id);
            if (booking == null) return;

            booking.Status = "Cancelled";

            await _db.Bookings.UpdateAsync(booking);
            await _db.CompleteAsync();
        }

        private IQueryable<BookingDetailsProjection> BuildBookingQuery(IQueryable<Booking> bookings)
        {
            return from booking in bookings
                   join room in _context.Rooms.AsNoTracking()
                       on booking.RoomId equals room.Id into roomJoin
                   from room in roomJoin.DefaultIfEmpty()
                   join hotel in _context.Hotels.AsNoTracking()
                       on room.HotelId equals hotel.Id into hotelJoin
                   from hotel in hotelJoin.DefaultIfEmpty()
                   select new BookingDetailsProjection(booking, room, hotel);
        }

        private static BookingDTO MapBookingDto(Booking booking, Room? room, Hotel? hotel)
        {
            return new BookingDTO
            {
                Id = booking.Id,
                UserId = booking.UserId,
                RoomId = booking.RoomId,
                HotelId = room?.HotelId,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                CreatedAt = booking.CreatedAt,
                AdultsCount = booking.AdultsCount,
                ChildrenCount = booking.ChildrenCount,
                Nights = Math.Max(0, (booking.CheckOut.Date - booking.CheckIn.Date).Days),
                TotalPrice = booking.TotalPrice,
                PricePerNight = room?.Price,
                CurrencyCode = room?.CurrencyCode ?? hotel?.CurrencyCode,
                Status = booking.Status,
                CallMe = booking.CallMe,
                SendEmail = booking.SendEmail,
                HotelName = hotel?.Name,
                HotelCity = hotel?.City,
                HotelCountry = hotel?.Country,
                HotelAddress = hotel?.Address,
                RoomTitle = room?.Title
            };
        }

        private sealed record BookingDetailsProjection(Booking Booking, Room? Room, Hotel? Hotel);
    }
}

