namespace e_trade_api.application;

public interface ITokenHandler
{
    Token CreateAccessToken(int minute);
}
