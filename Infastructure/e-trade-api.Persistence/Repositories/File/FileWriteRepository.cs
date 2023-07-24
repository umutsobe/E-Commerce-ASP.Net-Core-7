using e_trade_api.application;
using e_trade_api.Persistence.Contexts;

namespace e_trade_api.Persistence;

public class FileWriteRepository : WriteRepository<domain.File>, IFileWriteRepository
{
    public FileWriteRepository(ETradeApiDBContext context)
        : base(context) { }
}
