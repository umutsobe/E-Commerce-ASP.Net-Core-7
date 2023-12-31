namespace e_trade_api.application;

public interface IMailService
{
    Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true);
    Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true);
    Task SendPasswordResetMailAsync(string to, string userId, string resetToken);
    Task SendCompletedOrderMailAsync(
        string to,
        string orderCode,
        DateTime orderDate,
        string userName
    );
    Task SendEmailVerificationCode(string to, string code);
}
