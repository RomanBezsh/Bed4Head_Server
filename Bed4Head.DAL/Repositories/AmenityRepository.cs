using Bed4Head.DAL.EF;
using Bed4Head.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bed4Head.DAL.Repositories
{
    public class AmenityRepository : IRepository<Amenity>
    {
        private readonly AppDbContext _db;

        public AmenityRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Amenity>> GetAllAsync()
        {
            return await _db.Amenities.AsNoTracking().ToListAsync();
        }

        public async Task<Amenity?> GetByIdAsync(Guid id)
        {
            return await _db.Amenities.FindAsync(id);
        }

        public async Task AddAsync(Amenity entity)
        {
            await _db.Amenities.AddAsync(entity);
        }

        public async Task UpdateAsync(Amenity entity)
        {
            _db.Amenities.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _db.Amenities.FindAsync(id);
            if (existing != null)
            {
                _db.Amenities.Remove(existing);
            }
            else
            {
                var stub = new Amenity { Id = id };
                _db.Entry(stub).State = EntityState.Deleted;
            }
        }
    }
}