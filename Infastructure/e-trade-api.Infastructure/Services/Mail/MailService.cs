using System.Net;
using System.Net.Mail;
using System.Text;
using e_trade_api.application;
using Microsoft.Extensions.Configuration;

namespace e_trade_api.Infastructure;

public class MailService : IMailService
{
    readonly IConfiguration _configuration;

    public MailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
    {
        await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
    }

    public async Task SendMailAsync(
        string[] tos,
        string subject,
        string body,
        bool isBodyHtml = true
    )
    {
        MailMessage mail = new();
        mail.IsBodyHtml = isBodyHtml;
        foreach (var to in tos)
            mail.To.Add(to);
        mail.Subject = subject;
        mail.Body = body;
        mail.From = new(
            _configuration["Mail:Username"],
            "Sobe E Ticaret",
            System.Text.Encoding.UTF8
        );

        SmtpClient smtp = new();
        smtp.Credentials = new NetworkCredential(
            _configuration["Mail:Username"],
            _configuration["Mail:Password"]
        );
        smtp.Port = 587;
        smtp.EnableSsl = true;
        smtp.Host = _configuration["Mail:Host"];
        await smtp.SendMailAsync(mail);
    }

    public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
    {
        StringBuilder mail = new();
        mail.AppendLine(
            "Merhaba<br>Eğer yeni şifre talebinde bulunduysanız aşağıdaki linkten şifrenizi yenileyebilirsiniz.<br>"
        );
        mail.AppendLine(
            $"<strong><a target=\"_blank\" href=\"{_configuration["AngularClientUrl"]}/update-password/{userId}/{resetToken}\">Yeni şifre talebi için tıklayınız...</a></strong><br><br>"
        );
        mail.AppendLine(
            "<span style=\"font-size:12px;\">NOT: Eğer bu talep tarafınızca gerçekleştirilmemişse lütfen bu maili ciddiye almayınız.</span><br>Saygılarımızla...<br><br><br>Sobe E Ticaret"
        );

        await SendMailAsync(to, "Şifre Yenileme Talebi", mail.ToString());
    }
}
