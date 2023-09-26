namespace e_trade_api.application;

public interface IOrderService
{
    Task<CreateOrderResponseDTO> CreateOrderAsync(CreateOrder createOrder);
    Task<GetAllOrdersByFilterResponseDTO> GetAllOrdersByFilterAsync(
        GetAllOrdersByFilterRequestDTO model
    );
    Task<SingleOrder> GetOrderByIdAsync(string id);
    Task<(bool, CompletedOrderDTO)> CompleteOrderAsync(string id);
    Task<bool> IsOrderValid(string orderId);
}
