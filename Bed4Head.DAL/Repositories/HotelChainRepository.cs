using Bed4Head.DAL.EF;
using Bed4Head.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bed4Head.DAL.Repositories
{
    public class HotelChainRepository : IRepository<HotelChain>
    {
        private readonly AppDbContext _db;

        public HotelChainRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<HotelChain>> GetAllAsync()
        {
            return await _db.HotelChains.AsNoTracking().ToListAsync();
        }

        public async Task<HotelChain?> GetByIdAsync(Guid id)
        {
            return await _db.HotelChains.FindAsync(id);
        }

        public async Task AddAsync(HotelChain entity)
        {
            await _db.HotelChains.AddAsync(entity);
        }

        public async Task UpdateAsync(HotelChain entity)
        {
            _db.HotelChains.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _db.HotelChains.FindAsync(id);
            if (existing != null)
            {
                _db.HotelChains.Remove(existing);
            }
            else
            {
                var stub = new HotelChain { Id = id };
                _db.Entry(stub).State = EntityState.Deleted;
            }
        }
    }
}