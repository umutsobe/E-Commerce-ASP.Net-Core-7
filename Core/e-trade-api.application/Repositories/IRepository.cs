using e_trade_api.domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.application;

public interface IRepository<T>
    where T : BaseEntity
{
    DbSet<T> Table { get; } // bizim veritabanımız Table'a eşit. Table = e ticaret veritabanımız
}
