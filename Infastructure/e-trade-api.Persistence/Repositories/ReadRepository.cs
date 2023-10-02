using System.Linq.Expressions;
using e_trade_api.application;
using e_trade_api.domain.Entities.Common;
using e_trade_api.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.Persistence;

// Table = application IRepository'de tanımladığımız veritabanımız.

public class ReadRepository<T> : IReadRepository<T>
    where T : BaseEntity
{
    private readonly ETradeApiDBContext _context;

    public ReadRepository(ETradeApiDBContext context)
    {
        _context = context;
    }

    public DbSet<T> Table => _context.Set<T>(); //DbSet, veritabanı tablosunu temsil eder ve o tablodaki varlıklarla (kayıtlar) etkileşim sağlar. DbSet, veritabanına sorgular göndermek, varlık eklemek, güncellemek, silmek veya sorgulamak gibi işlemleri gerçekleştirmek için kullanılır.

    public IQueryable<T> GetAll(bool tracking = true)
    {
        var query = Table.AsQueryable();
        if (!tracking)
        {
            query = query.AsNoTracking();
        }
        return query;
    }

    public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
    {
        var query = Table.Where(method);
        if (!tracking)
        {
            query = query.AsNoTracking();
        }
        return query;
    }

    public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
    {
        var query = Table.AsQueryable();
        if (!tracking)
        {
            query = query.AsNoTracking();
        }
        return await query.FirstOrDefaultAsync(method);
    }

    public async Task<T> GetByIdAsync(string id, bool tracking = true)
    {
        var query = Table.AsQueryable();
        if (!tracking)
        {
            query = query.AsNoTracking();
        }
        return await query.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
    }
}
