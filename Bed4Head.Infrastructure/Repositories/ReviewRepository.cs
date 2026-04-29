using Bed4Head.Infrastructure.Data;
using Bed4Head.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bed4Head.Infrastructure.Repositories
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
            return await _db.Reviews
                .Include(r => r.User)   
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(Guid id)
        {
            return await _db.Reviews
                .Include(r => r.User)   
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
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

