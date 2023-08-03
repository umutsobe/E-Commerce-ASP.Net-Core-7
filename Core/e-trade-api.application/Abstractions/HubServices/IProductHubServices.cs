namespace e_trade_api.application;

public interface IProductHubServices
{
    Task ProductAddedMessageAsync(string message);
}
