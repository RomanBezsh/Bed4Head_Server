namespace Bed4Head.BLL.DTO
{
    public class PaymentMethodDTO
    {
        public Guid Id { get; set; }
        public required string CardType { get; set; }
        public required string LastFourDigits { get; set; }
        public string? ExpiryDate { get; set; }
        public bool IsPrimary { get; set; } 
        public Guid UserId { get; set; }
    }
}