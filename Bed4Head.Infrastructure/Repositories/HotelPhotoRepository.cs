using Bed4Head.Infrastructure.Data;
using Bed4Head.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bed4Head.Infrastructure.Repositories
{
    public class HotelPhotoRepository : IRepository<HotelPhoto>
    {
        private readonly AppDbContext _db;

        public HotelPhotoRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<HotelPhoto>> GetAllAsync()
        {
            return await _db.HotelPhotos.AsNoTracking().ToListAsync();
        }

        public async Task<HotelPhoto?> GetByIdAsync(Guid id)
        {
            return await _db.HotelPhotos.FindAsync(id);
        }

        public async Task AddAsync(HotelPhoto entity)
        {
            await _db.HotelPhotos.AddAsync(entity);
        }

        public async Task UpdateAsync(HotelPhoto entity)
        {
            _db.HotelPhotos.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _db.HotelPhotos.FindAsync(id);
            if (existing != null)
            {
                _db.HotelPhotos.Remove(existing);
            }
            else
            {
                var stub = new HotelPhoto { Id = id };
                _db.Entry(stub).State = EntityState.Deleted;
            }
        }
    }
}

