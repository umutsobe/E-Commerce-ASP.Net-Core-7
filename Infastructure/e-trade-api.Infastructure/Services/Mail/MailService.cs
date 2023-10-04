using System.Net;
using System.Net.Mail;
using System.Text;
using e_trade_api.application;
using Microsoft.Extensions.Configuration;

namespace e_trade_api.Infastructure;

public class MailService : IMailService
{
    private readonly IConfiguration _configuration;

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
            _configuration.GetValue<string>("Mail:Username"),
            _configuration.GetValue<string>("Mail:EmailHeader"),
            System.Text.Encoding.UTF8
        );

        SmtpClient smtp = new();
        smtp.Credentials = new NetworkCredential(
            _configuration.GetValue<string>("Mail:Username"),
            _configuration.GetValue<string>("Mail:Password")
        );
        smtp.Port = _configuration.GetValue<int>("Mail:Port");
        smtp.EnableSsl = true;
        smtp.Host = _configuration.GetValue<string>("Mail:Host");
        await smtp.SendMailAsync(mail);
    }

    public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
    {
        StringBuilder mail = new();
        mail.AppendLine(
            "Merhaba<br>Eğer yeni şifre talebinde bulunduysanız aşağıdaki linkten şifrenizi yenileyebilirsiniz.<br>"
        );
        mail.AppendLine(
            $"<strong><a target=\"_blank\" href=\"{_configuration.GetValue<string>("AngularClientUrl")}/update-password/{userId}/{resetToken}\">Yeni şifre talebi için tıklayınız...</a></strong><br><br>"
        );
        mail.AppendLine(
            "<span style=\"font-size:12px;\">NOT: Eğer bu talep tarafınızca gerçekleştirilmemişse lütfen bu maili ciddiye almayınız.</span><br>Saygılarımızla...<br><br><br>Sobe E Ticaret"
        );

        await SendMailAsync(to, "Şifre Yenileme Talebi", mail.ToString());
    }

    public async Task SendCompletedOrderMailAsync(
        string to,
        string orderCode,
        DateTime orderDate,
        string userName
    )
    {
        string mail =
            $"Sayın {userName} Merhaba<br>"
            + $"{orderDate} tarihinde vermiş olduğunuz {orderCode} kodlu siparişiniz onaylanmıştır. Yakın zamanda kargoya verilecektir. <br>Bizi tercih ettiğiniz için teşekkür ederiz";

        await SendMailAsync(to, $"{orderCode} Sipariş Numaralı Siparişiniz Tamamlandı", mail);
    }

    public async Task SendEmailVerificationCode(string to, string code)
    {
        string mail =
            $"Doğrulama kodu: {code} <br><br>Eğer bu mail isteğiniz dışında ise dikkate almayınız.";

        await SendMailAsync(to, "Email Doğrulama", mail);
    }
}
