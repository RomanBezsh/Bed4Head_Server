using Bed4Head.BLL.DTO;
using Bed4Head.BLL.Interfaces;
using Bed4Head.DAL.Entities;
using Bed4Head.DAL.Repositories;

namespace Bed4Head.BLL.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IUnitOfWork _db;

        public PaymentMethodService(IUnitOfWork db) => _db = db;

        public async Task<IEnumerable<PaymentMethodDTO>> GetByUserIdAsync(Guid userId)
        {
            var methods = await _db.PaymentMethods.GetAllAsync();
            return methods.Where(m => m.UserId == userId)
                          .Select(m => MapToDto(m));
        }

        public async Task<PaymentMethodDTO?> GetByIdAsync(Guid id)
        {
            var m = await _db.PaymentMethods.GetByIdAsync(id);
            return m == null ? null : MapToDto(m);
        }

        public async Task CreateAsync(PaymentMethodDTO dto)
        {
            var method = new PaymentMethod
            {
                Id = Guid.NewGuid(),
                CardType = dto.CardType,
                LastFourDigits = dto.LastFourDigits,
                ExpiryDate = dto.ExpiryDate,
                IsPrimary = dto.IsPrimary,
                UserId = dto.UserId
            };

            await _db.PaymentMethods.AddAsync(method);
            await _db.CompleteAsync();
        }

        public async Task UpdateAsync(PaymentMethodDTO dto)
        {
            var method = await _db.PaymentMethods.GetByIdAsync(dto.Id);
            if (method != null)
            {
                method.CardType = dto.CardType;
                method.ExpiryDate = dto.ExpiryDate;
                method.IsPrimary = dto.IsPrimary;

                await _db.PaymentMethods.UpdateAsync(method);
                await _db.CompleteAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await _db.PaymentMethods.DeleteAsync(id);
            await _db.CompleteAsync();
        }

        private static PaymentMethodDTO MapToDto(PaymentMethod m) => new PaymentMethodDTO
        {
            Id = m.Id,
            CardType = m.CardType,
            LastFourDigits = m.LastFourDigits,
            ExpiryDate = m.ExpiryDate,
            IsPrimary = m.IsPrimary,
            UserId = m.UserId
        };
    }
}