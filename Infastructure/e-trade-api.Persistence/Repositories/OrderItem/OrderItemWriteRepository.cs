using e_trade_api.application;
using e_trade_api.domain;
using e_trade_api.domain.Entities;
using e_trade_api.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.Persistence;

public class OrderItemWriteRepository : WriteRepository<OrderItem>, IOrderItemWriteRepository
{
    public OrderItemWriteRepository(ETradeApiDBContext context)
        : base(context) { }
}
