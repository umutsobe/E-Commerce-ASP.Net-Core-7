namespace e_trade_api.application;

public interface IOrderService
{
    Task CreateOrderAsync(CreateOrder createOrder);
}
