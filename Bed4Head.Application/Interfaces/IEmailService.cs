namespace Bed4Head.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendVerificationCodeAsync(string toEmail, string code);
        Task SendBookingConfirmationAsync(string toEmail, string hotelName, string roomTitle);
    }
}

