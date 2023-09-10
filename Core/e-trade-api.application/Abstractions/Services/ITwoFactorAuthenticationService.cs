namespace e_trade_api.application;

public interface ITwoFactorAuthenticationService
{
    Task<CreateCodeAndSendEmailResponse> CreateCodeAndSendEmail(string userId);
    Task<IsCodeValidResponseMessage> IsCodeValid(IsCodeValidRequest model);
    Task<bool> IsUserEmailConfirmed(string userId);
}
