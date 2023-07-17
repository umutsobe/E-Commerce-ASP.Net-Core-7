using e_trade_api.application;
using e_trade_api.domain.Entities;
using e_trade_api.Persistence.Contexts;

namespace e_trade_api.Persistence;

public class CustomerReadRepository : ReadRepository<Customer>, ICustomerReadRepository
{
    public CustomerReadRepository(ETradeApiDBContext context)
        : base(context) { }
}
