using System.Linq.Expressions;
using e_trade_api.domain.Entities.Common;

namespace e_trade_api.application;

public interface IReadRepository<T> : IRepository<T>
    where T : BaseEntity // buraya class yazsaydık BaseEntity olmayan class da gelebilirdi. BaseEntity diyerek türü sabitledik. zaten başka türde çalışmayacağız
{
    IQueryable<T> GetAll(bool tracking = true);
    IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true);
    Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true);
    Task<T> GetByIdAsync(string id, bool tracking = true);
}
// IQueryable veritabanından istediğimiz verileri çekmemize yarıyor. Ienumarable kullansaydık bütün verileri veritabanından almak zorunda olacaktık. IQueryable bunu veritabanında yapıyor
// asenkron işlemlerde Task<X> döndürülür kullanılır. bir şey döndürmeyeceksen "Task" sade kullanılır
//X= döndürülmek istenen tür. string, int, bool ...
