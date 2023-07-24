using Microsoft.AspNetCore.Http;

namespace e_trade_api.application;

public interface IStorage
{
    Task<List<(string fileName, string path)>> UploadAsync(
        string pathOrContainerName,
        IFormFileCollection files
    );
    Task DeleteAsync(string pathOrContainerName, string fileName);
    List<string> GetFiles(string pathOrContainerName);
    bool HasFile(string pathOrContainerName, string fileName);
}
