using e_trade_api.application;
using e_trade_api.domain;
using e_trade_api.Persistence.Contexts;

namespace e_trade_api.Persistence;

public class CompletedOrderWriteRepository
    : WriteRepository<CompletedOrder>,
        ICompletedOrderWriteRepository
{
    public CompletedOrderWriteRepository(ETradeApiDBContext context)
        : base(context) { }
}
