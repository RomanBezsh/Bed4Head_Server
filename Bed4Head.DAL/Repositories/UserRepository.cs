using Bed4Head.DAL.EF;
using Bed4Head.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bed4Head.DAL.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _db.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task AddAsync(User entity)
        {
            await _db.Users.AddAsync(entity);
        }

        public async Task UpdateAsync(User entity)
        {
            _db.Users.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _db.Users.FindAsync(id);
            if (existing != null)
            {
                _db.Users.Remove(existing);
            }
            else
            {
                var stub = new User { Id = id };
                _db.Entry(stub).State = EntityState.Deleted;
            }
        }
    }
}