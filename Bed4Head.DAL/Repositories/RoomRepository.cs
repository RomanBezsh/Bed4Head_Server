using Bed4Head.DAL.EF;
using Bed4Head.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bed4Head.DAL.Repositories
{
    public class RoomRepository : IRepository<Room>
    {
        private readonly AppDbContext _db;

        public RoomRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await _db.Rooms.AsNoTracking().ToListAsync();
        }

        public async Task<Room?> GetByIdAsync(Guid id)
        {
            return await _db.Rooms.FindAsync(id);
        }

        public async Task AddAsync(Room entity)
        {
            await _db.Rooms.AddAsync(entity);
        }

        public async Task UpdateAsync(Room entity)
        {
            _db.Rooms.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _db.Rooms.FindAsync(id);
            if (existing != null)
            {
                _db.Rooms.Remove(existing);
            }
            else
            {
                var stub = new Room { Id = id };
                _db.Entry(stub).State = EntityState.Deleted;
            }
        }
    }
}