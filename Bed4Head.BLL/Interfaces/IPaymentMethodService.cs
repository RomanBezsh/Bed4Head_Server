using Bed4Head.BLL.DTO;

namespace Bed4Head.BLL.Interfaces
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