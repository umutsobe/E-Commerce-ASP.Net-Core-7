using e_trade_api.application;
using e_trade_api.domain;
using e_trade_api.Persistence.Contexts;

namespace e_trade_api.Persistence;

public class BasketWriteRepository : WriteRepository<Basket>, IBasketWriteRepository
{
    public BasketWriteRepository(ETradeApiDBContext context)
        : base(context) { }
}
