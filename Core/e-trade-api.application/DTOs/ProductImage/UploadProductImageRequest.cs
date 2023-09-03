using Microsoft.AspNetCore.Http;

namespace e_trade_api.application;

public class UploadProductImageRequest
{
    public string ProductName { get; set; }
    public string ContainerName { get; set; }
    public IFormFileCollection Files { get; set; }
}
// string productUrlId,string ContainerName, IFormFileCollection files
