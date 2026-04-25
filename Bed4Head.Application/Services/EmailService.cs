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

            var senderEmail = _config["EmailSettings:SenderEmail"];
            var smtpServer = _config["EmailSettings:SmtpServer"];
            var password = _config["EmailSettings:Password"];
            var portValue = _config["EmailSettings:Port"];

            if (string.IsNullOrWhiteSpace(senderEmail) ||
                string.IsNullOrWhiteSpace(smtpServer) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException(
                    "EmailSettings are not configured. Set EmailSettings:SmtpServer, EmailSettings:Port, EmailSettings:SenderEmail, EmailSettings:Password.");
            }

            var port = 587;
            if (!string.IsNullOrWhiteSpace(portValue) && int.TryParse(portValue, out var parsedPort))
            {
                port = parsedPort;
            }

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Bed4Head", senderEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "Код подтверждения Bed4Head";

            var builder = new BodyBuilder
            {
                HtmlBody = $@"
                <html>
                <head>
                    <link href='https://fonts.googleapis.com/css2?family=Nunito+Sans:wght@400;700;800&display=swap' rel='stylesheet'>
                    <style>
                        body {{ font-family: 'Nunito Sans', Arial, sans-serif; }}
                    </style>
                </head>
                <body style='margin: 0; padding: 0; background-color: #f9f9f9;'>
                    <div style='font-family: ""Nunito Sans"", Arial, sans-serif; max-width: 400px; margin: 20px auto; border: 1px solid #f0f0f0; border-radius: 16px; padding: 40px 20px; text-align: center; background-color: #ffffff;'>
                        <h2 style='color: #222222; margin-bottom: 10px; font-weight: 800; font-size: 24px;'>Verify Your Account</h2>
                        <p style='color: #717171; font-size: 16px; margin-bottom: 30px; font-weight: 400;'>
                            Welcome to <strong>Bed4Head</strong>! <br> Use the verification code below to complete your registration:
                        </p>
                        
                        <div style='background-color: #f4f0ff; border-radius: 12px; padding: 25px; margin-bottom: 30px;'>
                            <h1 style='letter-spacing: 10px; color: #581ADB; font-size: 40px; margin: 0; font-weight: 800;'>{code}</h1>
                        </div>
                        
                        <p style='font-size: 14px; color: #717171; line-height: 1.6;'>
                            If you didn't sign up for a Bed4Head account, you can safely ignore this email.
                        </p>
                        
                        <hr style='border: 0; border-top: 1px solid #eee; margin: 30px 0;'>
                        
                        <p style='font-size: 12px; color: #222222; font-weight: 700; margin: 0; text-transform: uppercase; letter-spacing: 1px;'>
                            © {DateTime.Now.Year} Bed4Head Team
                        </p>
                    </div>
                </body>
                </html>"
            };

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(smtpServer, port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(senderEmail, password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendBookingConfirmationAsync(string toEmail, string hotelName, string roomTitle)
        {
            await Task.CompletedTask;
        }
    }
}
