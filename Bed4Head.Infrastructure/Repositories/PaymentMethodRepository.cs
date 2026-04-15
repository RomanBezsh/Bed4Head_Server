using Bed4Head.Infrastructure.Data;
using Bed4Head.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bed4Head.Infrastructure.Repositories
{
    public class PaymentMethodRepository : IRepository<PaymentMethod>
    {
        private readonly AppDbContext _db;

        public PaymentMethodRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<PaymentMethod>> GetAllAsync()
        {
            return await _db.PaymentMethods.AsNoTracking().ToListAsync();
        }

        public async Task<PaymentMethod?> GetByIdAsync(Guid id)
        {
            return await _db.PaymentMethods.FindAsync(id);
        }

        public async Task AddAsync(PaymentMethod entity)
        {
            await _db.PaymentMethods.AddAsync(entity);
        }

        public async Task UpdateAsync(PaymentMethod entity)
        {
            _db.PaymentMethods.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _db.PaymentMethods.FindAsync(id);
            if (existing != null)
            {
                _db.PaymentMethods.Remove(existing);
            }
            else
            {
                var stub = new PaymentMethod { Id = id, CardType = string.Empty, LastFourDigits = string.Empty };
                _db.Entry(stub).State = EntityState.Deleted;
            }
        }
    }
}

