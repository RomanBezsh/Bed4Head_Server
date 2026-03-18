using Bed4Head.DAL.EF;
using Bed4Head.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bed4Head.DAL.Repositories
{
    public class ReviewRepository : IRepository<Review>
    {
        private readonly AppDbContext _db;

        public ReviewRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _db.Reviews.AsNoTracking().ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(Guid id)
        {
            return await _db.Reviews.FindAsync(id);
        }

        public async Task AddAsync(Review entity)
        {
            await _db.Reviews.AddAsync(entity);
        }

        public async Task UpdateAsync(Review entity)
        {
            _db.Reviews.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _db.Reviews.FindAsync(id);
            if (existing != null)
            {
                _db.Reviews.Remove(existing);
            }
            else
            {
                var stub = new Review { Id = id };
                _db.Entry(stub).State = EntityState.Deleted;
            }
        }
    }
}