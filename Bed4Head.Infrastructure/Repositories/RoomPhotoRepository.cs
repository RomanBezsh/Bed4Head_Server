using Bed4Head.Infrastructure.Data;
using Bed4Head.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bed4Head.Infrastructure.Repositories
{
    public class RoomPhotoRepository : IRepository<RoomPhoto>
    {
        private readonly AppDbContext _db;

        public RoomPhotoRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<RoomPhoto>> GetAllAsync()
        {
            return await _db.RoomPhotos.AsNoTracking().ToListAsync();
        }

        public async Task<RoomPhoto?> GetByIdAsync(Guid id)
        {
            return await _db.RoomPhotos.FindAsync(id);
        }

        public async Task AddAsync(RoomPhoto entity)
        {
            await _db.RoomPhotos.AddAsync(entity);
        }

        public async Task UpdateAsync(RoomPhoto entity)
        {
            _db.RoomPhotos.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _db.RoomPhotos.FindAsync(id);
            if (existing != null)
            {
                _db.RoomPhotos.Remove(existing);
            }
            else
            {
                var stub = new RoomPhoto { Id = id, Url = string.Empty };
                _db.Entry(stub).State = EntityState.Deleted;
            }
        }
    }
}

