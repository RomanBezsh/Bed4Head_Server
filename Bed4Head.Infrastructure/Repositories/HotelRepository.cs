using Bed4Head.Infrastructure.Data;
using Bed4Head.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bed4Head.Infrastructure.Repositories
{
    public class HotelRepository : IRepository<Hotel>
    {
        private readonly AppDbContext _db;

        public HotelRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Hotel>> GetAllAsync()
        {
            return await _db.Hotels.AsNoTracking().ToListAsync();
        }

        public async Task<Hotel?> GetByIdAsync(Guid id)
        {
            return await _db.Hotels.FindAsync(id);
        }

        public async Task AddAsync(Hotel entity)
        {
            await _db.Hotels.AddAsync(entity);
        }

        public async Task UpdateAsync(Hotel entity)
        {
            _db.Hotels.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _db.Hotels.FindAsync(id);
            if (existing != null)
            {
                _db.Hotels.Remove(existing);
            }
            else
            {
                var stub = new Hotel { Id = id };
                _db.Entry(stub).State = EntityState.Deleted;
            }
        }
    }
}

