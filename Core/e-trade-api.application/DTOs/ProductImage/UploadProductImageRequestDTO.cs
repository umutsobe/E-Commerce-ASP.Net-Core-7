using Microsoft.AspNetCore.Http;

namespace e_trade_api.application;

public class UploadProductImageRequestDTO
{
    public string Id { get; set; }
    public IFormFileCollection? Files { get; set; }
}
