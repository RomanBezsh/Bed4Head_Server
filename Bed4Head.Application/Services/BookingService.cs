using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Bed4Head.Domain.Entities;
using Bed4Head.Infrastructure.Repositories;

namespace Bed4Head.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _db;

        public BookingService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<IEnumerable<BookingDTO>> GetAllAsync()
        {
            var bookings = await _db.Bookings.GetAllAsync();
            return bookings.Select(b => new BookingDTO
            {
                UserId = b.UserId,
                RoomId = b.RoomId,
                CheckIn = b.CheckIn,
                CheckOut = b.CheckOut,
                AdultsCount = b.AdultsCount,
                ChildrenCount = b.ChildrenCount,
                TotalPrice = b.TotalPrice,
                Status = b.Status,
                CallMe = b.CallMe,
                SendEmail = b.SendEmail
            });
        }

        public async Task<BookingDTO?> GetByIdAsync(Guid id)
        {
            var b = await _db.Bookings.GetByIdAsync(id);
            if (b == null) return null;

            return new BookingDTO
            {
                UserId = b.UserId,
                RoomId = b.RoomId,
                CheckIn = b.CheckIn,
                CheckOut = b.CheckOut,
                AdultsCount = b.AdultsCount,
                ChildrenCount = b.ChildrenCount,
                TotalPrice = b.TotalPrice,
                Status = b.Status,
                CallMe = b.CallMe,
                SendEmail = b.SendEmail
            };
        }

        public async Task<IEnumerable<BookingDTO>> GetByUserIdAsync(Guid userId)
        {
            var all = await _db.Bookings.GetAllAsync();
            return all
                .Where(b => b.UserId == userId)
                .Select(b => new BookingDTO
                {
                    UserId = b.UserId,
                    RoomId = b.RoomId,
                    CheckIn = b.CheckIn,
                    CheckOut = b.CheckOut,
                    AdultsCount = b.AdultsCount,
                    ChildrenCount = b.ChildrenCount,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status,
                    CallMe = b.CallMe,
                    SendEmail = b.SendEmail
                });
        }

        public async Task CreateAsync(CreateBookingDTO dto, Guid userId)
        {
            var existingBookings = (await _db.Bookings.GetAllAsync())
                .Where(b => b.RoomId == dto.RoomId &&
                            b.Status != "Cancelled" &&
                            dto.CheckIn < b.CheckOut &&
                            dto.CheckOut > b.CheckIn);

            if (existingBookings.Any())
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
    }
}

