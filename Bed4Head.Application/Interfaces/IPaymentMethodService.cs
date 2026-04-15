using Bed4Head.Application.DTOs;

namespace Bed4Head.Application.Interfaces
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<PaymentMethodDTO>> GetByUserIdAsync(Guid userId);
        Task<PaymentMethodDTO?> GetByIdAsync(Guid id);
        Task CreateAsync(PaymentMethodDTO dto);
        Task UpdateAsync(PaymentMethodDTO dto);
        Task DeleteAsync(Guid id);
    }
}

