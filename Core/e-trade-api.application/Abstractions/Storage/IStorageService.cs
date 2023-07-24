namespace e_trade_api.application;

public interface IStorageService : IStorage
{
    public string StorageName { get; }
}
