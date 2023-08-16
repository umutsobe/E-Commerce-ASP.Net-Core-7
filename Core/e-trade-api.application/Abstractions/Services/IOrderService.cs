namespace e_trade_api.application;

public interface IOrderService
{
    Task CreateOrderAsync(CreateOrder createOrder);
    Task<ListOrder> GetAllOrdersAsync(int page, int size);
    Task<SingleOrder> GetOrderByIdAsync(string id);
    Task CompleteOrder(string id);
}
