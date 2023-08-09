namespace e_trade_api.application;

public interface IOrderHubService
{
    Task OrderAddedMessageAsync(string message);
}
