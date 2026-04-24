using Bed4Head.Application.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Bed4Head.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendVerificationCodeAsync(string toEmail, string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Verification code is required.", nameof(code));
            }

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Bed4Head", _config["EmailSettings:SenderEmail"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "Код подтверждения Bed4Head";

            var builder = new BodyBuilder
            {
                HtmlBody = $@"
                <div style='font-family: Arial; text-align: center; border: 1px solid #eee; padding: 20px;'>
                    <h2 style='color: #6366f1;'>Добро пожаловать в Bed4Head!</h2>
                    <p>Твой код подтверждения:</p>
                    <h1 style='letter-spacing: 5px; color: #1e1e1e;'>{code}</h1>
                    <p style='font-size: 12px; color: #888;'>Если ты не регистрировался у нас, просто удали это письмо.</p>
                </div>"
            };

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config["EmailSettings:SmtpServer"], 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["EmailSettings:SenderEmail"], _config["EmailSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendBookingConfirmationAsync(string toEmail, string hotelName, string roomTitle)
        {
            await Task.CompletedTask;
        }
    }
}
