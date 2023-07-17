using e_trade_api.domain.Entities.Common;

namespace e_trade_api.application;

public interface IWriteRepository<T> : IRepository<T>
    where T : BaseEntity // buraya class yazsaydık BaseEntity olmayan class da gelebilirdi. BaseEntity diyerek türü sabitledik. zaten başka türde çalışmayacağız
{
    Task<bool> AddAsync(T model);
    Task<bool> AddRangeAsync(List<T> datas);
    bool Remove(T model);
    Task<bool> RemoveAsync(string id);
    bool RemoveRange(List<T> datas);
    bool Update(T model);
    Task<int> SaveAsync();
}
// asenkron işlemlerde Task<X> döndürülür kullanılır. bir şey döndürmeyeceksen "Task" sade kullanılır
//X= döndürülmek istenen tür. string, int, bool ...
