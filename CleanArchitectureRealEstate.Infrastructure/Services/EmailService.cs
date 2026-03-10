using CleanArchitectureRealEstate.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace CleanArchitectureRealEstate.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendPasswordResetEmailAsync(string email, string resetToken, CancellationToken cancellationToken = default)
        {
            var resetLink = $"{_configuration["FrontendUrl"]}/reset-password?token={resetToken}";
            
            var subject = "Şifre Sıfırlama Talebi";
            var body = $@"
                <h2>Şifre Sıfırlama</h2>
                <p>Şifrenizi sıfırlamak için aşağıdaki linke tıklayın:</p>
                <a href='{resetLink}'>Şifremi Sıfırla</a>
                <p>Bu link 1 saat geçerlidir.</p>
                <p>Eğer bu talebi siz yapmadıysanız, bu e-postayı görmezden gelebilirsiniz.</p>
            ";

            await SendEmailAsync(email, subject, body, cancellationToken);
        }

        public async Task SendEDevletVerificationEmailAsync(string email, string verificationUrl, CancellationToken cancellationToken = default)
        {
            var subject = "e-Devlet Doğrulama";
            var body = $@"
                <h2>e-Devlet ile Hesap Doğrulama</h2>
                <p>Hesabınızı doğrulamak için aşağıdaki linke tıklayın:</p>
                <a href='{verificationUrl}'>e-Devlet ile Doğrula</a>
                <p>Yasal gereklilikler nedeniyle hesabınızı kullanabilmek için e-Devlet doğrulaması yapmanız gerekmektedir.</p>
            ";

            await SendEmailAsync(email, subject, body, cancellationToken);
        }

        private async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken)
        {
            var smtpHost = _configuration["Email:SmtpHost"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
            var smtpUser = _configuration["Email:SmtpUser"];
            var smtpPass = _configuration["Email:SmtpPass"];
            var fromEmail = _configuration["Email:FromEmail"] ?? smtpUser;

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(smtpUser, smtpPass)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail!),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);

            await client.SendMailAsync(mailMessage, cancellationToken);
        }
    }
}
