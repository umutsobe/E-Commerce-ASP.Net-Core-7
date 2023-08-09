using e_trade_api.application;
using e_trade_api.domain;
using e_trade_api.Persistence.Contexts;

namespace e_trade_api.Persistence;

public class BasketItemWriteRepository : WriteRepository<BasketItem>, IBasketItemWriteRepository
{
    public BasketItemWriteRepository(ETradeApiDBContext context)
        : base(context) { }
}
