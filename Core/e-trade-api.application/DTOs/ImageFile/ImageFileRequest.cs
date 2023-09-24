using Microsoft.AspNetCore.Http;

namespace e_trade_api.application;

public class ImageFileRequest
{
    public string ContainerName { get; set; }
    public string Definition { get; set; }
    public IFormFileCollection Files { get; set; }
}
