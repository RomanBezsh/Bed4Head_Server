using Bed4Head.DAL.EF;
using Bed4Head.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bed4Head.DAL.Repositories
{
    public class HotelFaqRepository : IRepository<HotelFaq>
    {
        private readonly AppDbContext _db;

        public HotelFaqRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<HotelFaq>> GetAllAsync()
        {
            return await _db.HotelFaqs.AsNoTracking().ToListAsync();
        }

        public async Task<HotelFaq?> GetByIdAsync(Guid id)
        {
            return await _db.HotelFaqs.FindAsync(id);
        }

        public async Task AddAsync(HotelFaq entity)
        {
            await _db.HotelFaqs.AddAsync(entity);
        }

        public async Task UpdateAsync(HotelFaq entity)
        {
            _db.HotelFaqs.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _db.HotelFaqs.FindAsync(id);
            if (existing != null)
            {
                _db.HotelFaqs.Remove(existing);
            }
            else
            {
                var stub = new HotelFaq 
                { 
                    Id = id, 
                    Question = string.Empty, 
                    Answer = string.Empty
                };
                _db.Entry(stub).State = EntityState.Deleted;
            }
        }
    }
}