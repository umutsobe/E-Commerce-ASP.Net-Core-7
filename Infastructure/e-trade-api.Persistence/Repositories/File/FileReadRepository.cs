using e_trade_api.application;
using e_trade_api.Persistence.Contexts;

namespace e_trade_api.Persistence;

public class FileReadRepository : ReadRepository<domain.File>, IFileReadRepository
{
    public FileReadRepository(ETradeApiDBContext context)
        : base(context) { }
}
