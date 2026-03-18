using Bed4Head.DAL.EF;
using Bed4Head.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bed4Head.DAL.Repositories
{
    public class NearbyPlaceRepository : IRepository<NearbyPlace>
    {
        private readonly AppDbContext _db;

        public NearbyPlaceRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<NearbyPlace>> GetAllAsync()
        {
            return await _db.NearbyPlaces.AsNoTracking().ToListAsync();
        }

        public async Task<NearbyPlace?> GetByIdAsync(Guid id)
        {
            return await _db.NearbyPlaces.FindAsync(id);
        }

        public async Task AddAsync(NearbyPlace entity)
        {
            await _db.NearbyPlaces.AddAsync(entity);
        }

        public async Task UpdateAsync(NearbyPlace entity)
        {
            _db.NearbyPlaces.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _db.NearbyPlaces.FindAsync(id);
            if (existing != null)
            {
                _db.NearbyPlaces.Remove(existing);
            }
            else
            {
                var stub = new NearbyPlace
                {
                    Id = id,
                    Name = string.Empty,
                    PlaceType = string.Empty
                };
                _db.Entry(stub).State = EntityState.Deleted;
            }
        }
    }
}