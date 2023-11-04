namespace e_trade_api.application;

public interface ICloudflareService
{
    Task<bool> PurgeEverythingCache();
}
