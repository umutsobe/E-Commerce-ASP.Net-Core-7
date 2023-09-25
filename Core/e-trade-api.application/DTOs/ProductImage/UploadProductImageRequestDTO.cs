using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http;

namespace e_trade_api.application;

public class UploadImageRequestDTO
{
    public string? Id { get; set; }
    public string? Definition { get; set; }
    public IFormFileCollection? Files { get; set; }
}
