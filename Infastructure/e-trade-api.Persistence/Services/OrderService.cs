using System.Text;
using e_trade_api.application;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.Persistence;

public class OrderService : IOrderService
{
    readonly IOrderWriteRepository _orderWriteRepository;
    readonly IOrderReadRepository _orderReadRepository;
    readonly IOrderItemWriteRepository _orderItemWriteRepository;

    public OrderService(
        IOrderWriteRepository orderWriteRepository,
        IOrderItemWriteRepository orderItemWriteRepository,
        IOrderReadRepository orderReadRepository
    )
    {
        _orderWriteRepository = orderWriteRepository;
        _orderItemWriteRepository = orderItemWriteRepository;
        _orderReadRepository = orderReadRepository;
    }

    public async Task CreateOrderAsync(CreateOrder createOrder)
    {
        var orderId = Guid.NewGuid();

        string orderCode = await GenerateUniqueOrderCodeAsync();

        await _orderWriteRepository.AddAsync(
            new()
            {
                Id = orderId,
                UserId = createOrder.UserId,
                Description = createOrder.Description,
                Adress = createOrder.Address,
                OrderCode = orderCode
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

    public async Task<ListOrder> GetAllOrdersAsync(int page, int size)
    {
        var query = _orderReadRepository.Table
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product);

        var data = query.Skip(page * size).Take(size);

        return new()
        {
            TotalOrderCount = await query.CountAsync(),
            Orders = await data.Select(
                    o =>
                        new
                        {
                            Id = o.Id,
                            CreatedDate = o.CreatedDate,
                            OrderCode = o.OrderCode,
                            TotalPrice = o.OrderItems.Sum(bi => bi.Product.Price * bi.Quantity),
                            UserName = o.User.UserName
                        }
                )
                .ToListAsync()
        };
    }

    public async Task<SingleOrder> GetOrderByIdAsync(string id)
    {
        var data = await _orderReadRepository.Table
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));

        return new()
        {
            Id = data.Id.ToString(),
            OrderItems = data.OrderItems.Select(
                oi =>
                    new
                    {
                        oi.Product.Name,
                        oi.Product.Price,
                        oi.Product.Id,
                        oi.Quantity
                    }
            ),
            Address = data.Adress,
            CreatedDate = data.CreatedDate,
            Description = data.Description,
            OrderCode = data.OrderCode,
        };
    }

    private async Task<string> GenerateUniqueOrderCodeAsync()
    {
        Random random = new();
        StringBuilder orderCodeBuilder = new();

        for (int i = 0; i < 10; i++)
        {
            int randomNumber = random.Next(0, 10);
            orderCodeBuilder.Append(randomNumber);
        }

        string orderCode = orderCodeBuilder.ToString();

        bool isOrderCodeUnique = await IsOrderCodeUniqueAsync(orderCode);

        // If the generated order code is not unique, regenerate it.
        while (!isOrderCodeUnique)
        {
            for (int i = 0; i < 10; i++)
            {
                int randomNumber = random.Next(0, 10);
                orderCodeBuilder[i] = (char)('0' + randomNumber);
            }

            orderCode = orderCodeBuilder.ToString();
            isOrderCodeUnique = await IsOrderCodeUniqueAsync(orderCode);
        }

        return orderCode;
    }

    private async Task<bool> IsOrderCodeUniqueAsync(string orderCode)
    {
        var existingOrder = await _orderReadRepository.Table.FirstOrDefaultAsync(
            o => o.OrderCode == orderCode
        );

        return existingOrder == null;
    }
}
