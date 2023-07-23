using Microsoft.AspNetCore.Http;

namespace e_trade_api.application;

public interface IFileService
{
    Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files);
    Task<bool> CopyFileAsync(string path, IFormFile file);
}
