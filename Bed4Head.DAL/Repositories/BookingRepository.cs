using Bed4Head.DAL.EF;
using Bed4Head.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bed4Head.DAL.Repositories
{
    public class BookingRepository : IRepository<Booking>
    {
        private readonly AppDbContext _db;

        public BookingRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _db.Bookings.AsNoTracking().ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(Guid id)
        {
            return await _db.Bookings.FindAsync(id);
        }

        public async Task AddAsync(Booking entity)
        {
            await _db.Bookings.AddAsync(entity);
        }

        public async Task UpdateAsync(Booking entity)
        {
            _db.Bookings.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _db.Bookings.FindAsync(id);
            if (existing != null)
            {
                _db.Bookings.Remove(existing);
            }
            else
            {
                var stub = new Booking { Id = id };
                _db.Entry(stub).State = EntityState.Deleted;
            }
        }
    }
}