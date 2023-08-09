using e_trade_api.application;

namespace e_trade_api.Persistence;

public class OrderService : IOrderService
{
    readonly IOrderWriteRepository _orderWriteRepository;
    readonly IOrderItemWriteRepository _orderItemWriteRepository;

    public OrderService(
        IOrderWriteRepository orderWriteRepository,
        IOrderItemWriteRepository orderItemWriteRepository
    )
    {
        _orderWriteRepository = orderWriteRepository;
        _orderItemWriteRepository = orderItemWriteRepository;
    }

    public async Task CreateOrderAsync(CreateOrder createOrder)
    {
        var orderId = Guid.NewGuid();

        await _orderWriteRepository.AddAsync(
            new()
            {
                Id = orderId,
                UserId = createOrder.UserId,
                Description = createOrder.Description,
                Adress = createOrder.Address
            }
        );

        await _orderWriteRepository.SaveAsync();

        foreach (var orderItem in createOrder.OrderItems)
        {
            await _orderItemWriteRepository.AddAsync(
                new()
                {
                    Id = Guid.NewGuid(),
                    OrderId = orderId,
                    ProductId = Guid.Parse(orderItem.ProductId),
                    Price = orderItem.Price,
                    Quantity = orderItem.Quantity
                }
            );
        }

        await _orderItemWriteRepository.SaveAsync();
    }
}
