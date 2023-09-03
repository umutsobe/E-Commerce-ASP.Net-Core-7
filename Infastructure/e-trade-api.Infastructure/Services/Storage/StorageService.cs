using e_trade_api.application;
using Microsoft.AspNetCore.Http;

namespace e_trade_api.Infastructure;

public class StorageService : IStorageService
{
    private readonly IStorageService _storageService;

    public StorageService(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public string StorageName
    {
        get => _storageService.GetType().Name;
    }

    public async Task DeleteAsync(string ContainerName, string fileName)
    {
        await _storageService.DeleteAsync(ContainerName, fileName);
    }

    public List<string> GetFiles(string ContainerName)
    {
        return _storageService.GetFiles(ContainerName);
    }

    public async Task<bool> HasFile(string ContainerName, string fileName)
    {
        return await _storageService.HasFile(ContainerName, fileName);
    }

    public async Task<List<StorageFile>> UploadAsync(UploadProductImageRequest model)
    {
        return await _storageService.UploadAsync(model);
    }
}
