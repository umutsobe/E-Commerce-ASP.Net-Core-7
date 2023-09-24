namespace e_trade_api.application;

public interface IStorageService
{
    public string StorageName { get; }
    Task<List<StorageFile>> UploadProductImageAsync(UploadProductImageRequest model);
    Task DeleteAsync(string ContainerName, string fileName);
    List<string> GetFiles(string ContainerName);
    Task<bool> HasFile(string ContainerName, string fileName);
    Task<List<StorageFile>> UploadImageAsync(ImageFileRequest model);
}
