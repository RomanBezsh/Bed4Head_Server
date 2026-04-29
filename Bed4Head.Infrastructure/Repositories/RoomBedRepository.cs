using Bed4Head.Infrastructure.Data;
using Bed4Head.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bed4Head.Infrastructure.Repositories
{
    public class RoomBedRepository : IRepository<RoomBed>
    {
        private readonly AppDbContext _db;

        public RoomBedRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<RoomBed>> GetAllAsync()
        {
            return await _db.RoomBeds.AsNoTracking().ToListAsync();
        }

        public async Task<RoomBed?> GetByIdAsync(Guid id)
        {
            return await _db.RoomBeds.FindAsync(id);
        }

        public async Task AddAsync(RoomBed entity)
        {
            await _db.RoomBeds.AddAsync(entity);
        }

        public async Task UpdateAsync(RoomBed entity)
        {
            _db.RoomBeds.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _db.RoomBeds.FindAsync(id);
            if (existing != null)
            {
                _db.RoomBeds.Remove(existing);
            }
            else
            {
                var stub = new RoomBed { Id = id };
                _db.Entry(stub).State = EntityState.Deleted;
            }
        }
    }
}