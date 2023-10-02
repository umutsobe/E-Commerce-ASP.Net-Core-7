using System.Text;
using e_trade_api.application;
using e_trade_api.domain;
using e_trade_api.domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.Persistence;

public class OrderService : IOrderService
{
    readonly IOrderWriteRepository _orderWriteRepository;
    readonly IOrderReadRepository _orderReadRepository;
    readonly IOrderItemWriteRepository _orderItemWriteRepository;
    readonly ICompletedOrderWriteRepository _completedOrderWriteRepository;
    readonly ICompletedOrderReadRepository _completedOrderReadRepository;
    readonly IBasketItemWriteRepository _basketItemWriteRepository;
    readonly IBasketItemReadRepository _basketItemReadRepository;
    readonly IBasketReadRepository _basketReadRepository;
    readonly IProductReadRepository _productReadRepository;
    readonly IHttpContextAccessor _httpContextAccessor;

    public OrderService(
        IOrderWriteRepository orderWriteRepository,
        IOrderItemWriteRepository orderItemWriteRepository,
        IOrderReadRepository orderReadRepository,
        ICompletedOrderWriteRepository completedOrderWriteRepository,
        ICompletedOrderReadRepository completedOrderReadRepository,
        IBasketItemWriteRepository basketItemWriteRepository,
        IBasketReadRepository basketReadRepository,
        IBasketItemReadRepository basketItemReadRepository,
        IProductReadRepository productReadRepository,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _orderWriteRepository = orderWriteRepository;
        _orderItemWriteRepository = orderItemWriteRepository;
        _orderReadRepository = orderReadRepository;
        _completedOrderWriteRepository = completedOrderWriteRepository;
        _completedOrderReadRepository = completedOrderReadRepository;
        _basketItemWriteRepository = basketItemWriteRepository;
        _basketReadRepository = basketReadRepository;
        _basketItemReadRepository = basketItemReadRepository;
        _productReadRepository = productReadRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<(bool, CompletedOrderDTO)> CompleteOrderAsync(string id)
    {
        // Order order = await _orderReadRepository.GetByIdAsync(id);
        Order? order = await _orderReadRepository.Table
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));

        if (order != null)
        {
            bool completeOrder = await _completedOrderReadRepository.Table
                .Select(co => co.OrderId == Guid.Parse(id))
                .FirstOrDefaultAsync();

            if (!completeOrder)
            {
                await _completedOrderWriteRepository.AddAsync(new() { OrderId = Guid.Parse(id) });
                return (
                    await _completedOrderWriteRepository.SaveAsync() > 0,
                    new()
                    {
                        OrderCode = order.OrderCode,
                        OrderDate = order.CreatedDate,
                        Username = order.User.UserName,
                        EMail = order.User.Email
                    }
                );
            }
            else
                throw new Exception("Order Zaten Onaylandı");
        }
        return (false, null);
    }

    public async Task<CreateOrderResponseDTO> CreateOrderAsync(CreateOrder createOrder)
    {
        string? userId = _httpContextAccessor
            ?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId")
            ?.Value;

        if (userId == null)
            throw new Exception("User Not Found");

        var orderId = Guid.NewGuid();

        string orderCode = await GenerateUniqueOrderCodeAsync();

        await _orderWriteRepository.AddAsync(
            new()
            {
                Id = orderId,
                UserId = userId,
                Description = createOrder.Description,
                Adress = createOrder.Address,
                OrderCode = orderCode
            }
        );
        await _orderWriteRepository.SaveAsync();

        if (createOrder.OrderItems.Count < 1)
            return new() { Succeeded = false, Message = "Unable to process an empty order" };

        Dictionary<Product, int> productOriginalStock = new(); //tüm ürünlerin orijinal stok miktarını tutuyor, eğer stok hatası alırsak stok eski haline dönecek

        foreach (var orderItem in createOrder.OrderItems)
        {
            Product? product = await _productReadRepository.Table.FirstOrDefaultAsync(
                p => p.Id == Guid.Parse(orderItem.ProductId)
            );

            if (product != null)
            {
                productOriginalStock.Add(product, product.Stock);

                product.TotalOrderNumber += orderItem.Quantity;

                if (orderItem.Quantity <= product.Stock)
                {
                    bool result = await _orderItemWriteRepository.AddAsync(
                        new()
                        {
                            Id = Guid.NewGuid(),
                            OrderId = orderId,
                            ProductId = Guid.Parse(orderItem.ProductId),
                            Price = orderItem.Price,
                            Quantity = orderItem.Quantity
                        }
                    );
                    if (result)
                    {
                        product.Stock -= orderItem.Quantity;
                    }
                }
                else //sipariş verirken herhangi bir ürün stok miktarında hata alırsak. her şeyi eski haline getir.
                {
                    foreach (var _product in productOriginalStock) //ürünlerin stoklarını eski haline getir
                    {
                        Product updateProduct = _product.Key;
                        updateProduct.Stock = _product.Value;
                    }

                    await _orderWriteRepository.RemoveAsync(orderId.ToString());
                    await _orderWriteRepository.SaveAsync();
                    return new()
                    {
                        Succeeded = false,
                        Message =
                            "Some items in your order are out of stock. Please review your cart again."
                    };
                }
            }
            else
            {
                // Ürün bulunamadı, hata işleme yapılabilir
                return new() { Succeeded = false, Message = "Product not found" };
            }
        }

        //siparişte hata alırsak buraya gelmeyeceğiz zaten
        Basket? basket = await _basketReadRepository.Table.FirstOrDefaultAsync(
            b => b.UserId == userId
        );

        if (basket != null)
        {
            List<BasketItem>? basketItems = await _basketItemReadRepository.Table
                .Where(bi => bi.BasketId == basket.Id)
                .ToListAsync();

            _basketItemWriteRepository.RemoveRange(basketItems);
        }

        await _orderItemWriteRepository.SaveAsync();
        return new()
        {
            Succeeded = true,
            OrderCode = orderCode,
            OrderId = orderId.ToString()
        };
    }

    public async Task<GetAllOrdersByFilterResponseDTO> GetAllOrdersByFilterAsync(
        GetAllOrdersByFilterRequestDTO model
    )
    {
        var query = _orderReadRepository.Table
            .Include(o => o.OrderItems)
            .Include(o => o.User)
            .Include(o => o.CompletedOrder)
            .AsQueryable();

        if (!string.IsNullOrEmpty(model.UsernameKeyword))
            query = query.Where(
                o => EF.Functions.Like(o.User.UserName, $"%{model.UsernameKeyword}%")
            );
        if (!string.IsNullOrEmpty(model.OrderCodeKeyword))
            query = query.Where(o => EF.Functions.Like(o.OrderCode, $"%{model.OrderCodeKeyword}%"));

        if (model.IsConfirmed.HasValue)
        {
            if (model.IsConfirmed == true)
                query = query.Where(o => o.CompletedOrder != null);
            else
                query = query.Where(o => o.CompletedOrder == null);
        }

        var totalOrderCount = await query.CountAsync();

        if (!string.IsNullOrEmpty(model.Sort))
        {
            if (model.Sort == "new")
                query = query.OrderByDescending(o => o.CreatedDate);
            else if (model.Sort == "old")
                query = query.OrderBy(o => o.CreatedDate);
            else if (model.Sort == "priceDesc")
                query = query.OrderByDescending(o => o.OrderItems.Sum(item => item.Price));
            else if (model.Sort == "priceAsc")
                query = query.OrderBy(o => o.OrderItems.Sum(item => item.Price));
        }
        else
            query = query.OrderByDescending(p => p.CreatedDate);

        query = query.Skip(model.Page * model.Size).Take(model.Size);

        List<GetOrderByFilter> filteredOrders = await query
            .Select(
                o =>
                    new GetOrderByFilter
                    {
                        Id = o.Id.ToString(),
                        Completed = o.CompletedOrder != null,
                        CreatedDate = o.CreatedDate,
                        OrderCode = o.OrderCode,
                        TotalPrice = o.OrderItems.Sum(item => item.Price),
                        Username = o.User.UserName
                    }
            )
            .ToListAsync();

        return new() { Orders = filteredOrders, TotalOrderCount = totalOrderCount };
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

    public async Task<bool> IsOrderValid(string orderCode)
    {
        Order? order = await _orderReadRepository.Table.FirstOrDefaultAsync(
            o => o.OrderCode == orderCode
        );

        if (order == null)
            return false;

        return true;
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
