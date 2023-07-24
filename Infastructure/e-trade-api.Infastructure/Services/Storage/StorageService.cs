using e_trade_api.application;
using Microsoft.AspNetCore.Http;

namespace e_trade_api.Infastructure;

public class StorageService : IStorageService
{
    private readonly IStorage _storage;

    public StorageService(IStorage storage)
    {
        _storage = storage;
    }

    public string StorageName
    {
        get => _storage.GetType().Name;
    }

    public async Task DeleteAsync(string pathOrContainerName, string fileName)
    {
        await _storage.DeleteAsync(pathOrContainerName, fileName);
    }

    public List<string> GetFiles(string pathOrContainerName)
    {
        return _storage.GetFiles(pathOrContainerName);
    }

    public bool HasFile(string pathOrContainerName, string fileName)
    {
        return _storage.HasFile(pathOrContainerName, fileName);
    }

    public async Task<List<(string fileName, string path)>> UploadAsync(
        string pathOrContainerName,
        IFormFileCollection files
    )
    {
        return await _storage.UploadAsync(pathOrContainerName, files);
    }
}
