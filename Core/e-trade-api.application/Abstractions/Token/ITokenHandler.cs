namespace e_trade_api.application;

public interface ITokenHandler
{
    Task<Token> CreateAccessToken(int minute, string userId);
}
