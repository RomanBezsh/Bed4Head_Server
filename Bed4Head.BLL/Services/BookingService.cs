using Bed4Head.BLL.DTO;
using Bed4Head.BLL.Interfaces;
using Bed4Head.DAL.Entities;
using Bed4Head.DAL.Repositories;

namespace Bed4Head.BLL.Services
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
                Id = b.Id,
                CheckIn = b.CheckIn,
                CheckOut = b.CheckOut,
                TotalPrice = b.TotalPrice,
                Status = b.Status,
                UserId = b.UserId,
                RoomId = b.RoomId
            });
        }

        public async Task<BookingDTO?> GetByIdAsync(Guid id)
        {
            var b = await _db.Bookings.GetByIdAsync(id);
            if (b == null) return null;

            return new BookingDTO
            {
                Id = b.Id,
                CheckIn = b.CheckIn,
                CheckOut = b.CheckOut,
                TotalPrice = b.TotalPrice,
                Status = b.Status,
                UserId = b.UserId,
                RoomId = b.RoomId
            };
        }

        public async Task<IEnumerable<BookingDTO>> GetByUserIdAsync(Guid userId)
        {
            var all = await _db.Bookings.GetAllAsync();
            return all.Where(b => b.UserId == userId)
                      .Select(b => new BookingDTO { /* маппинг */ });
        }

        public async Task CreateAsync(BookingDTO dto)
        {
            var room = await _db.Rooms.GetByIdAsync(dto.RoomId);
            if (room == null) throw new Exception("Room not found");

            var days = (dto.CheckOut - dto.CheckIn).Days;
            var totalPrice = days > 0 ? (decimal)days * room.Price : room.Price;

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut,
                TotalPrice = dto.TotalPrice == 0 ? totalPrice : dto.TotalPrice,
                Status = "Pending",
                UserId = dto.UserId,
                RoomId = dto.RoomId, 
                CreatedAt = DateTime.UtcNow
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

        public async Task DeleteAsync(Guid id)
        {
            await _db.Bookings.DeleteAsync(id);
            await _db.CompleteAsync();
        }
    }
}