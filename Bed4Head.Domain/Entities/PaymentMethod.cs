namespace Bed4Head.Domain.Entities
{
    public class PaymentMethod
    {
        public Guid Id { get; set; }

        public required string CardType { get; set; }

        public required string LastFourDigits { get; set; }

        public string? ExpiryDate { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public bool IsPrimary { get; set; }
    }
}

