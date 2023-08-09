using e_trade_api.application;
using e_trade_api.domain;
using e_trade_api.Persistence.Contexts;

namespace e_trade_api.Persistence;

public class OrderItemReadRepository : ReadRepository<OrderItem>, IOrderItemReadRepository
{
    public OrderItemReadRepository(ETradeApiDBContext context)
        : base(context) { }
}
