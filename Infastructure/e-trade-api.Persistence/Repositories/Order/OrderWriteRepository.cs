using e_trade_api.application;
using e_trade_api.domain.Entities;
using e_trade_api.Persistence.Contexts;

namespace e_trade_api.Persistence;

public class OrderWriteRepository : WriteRepository<Order>, IOrderWriteRepository
{
    public OrderWriteRepository(ETradeApiDBContext context)
        : base(context) { }
}
