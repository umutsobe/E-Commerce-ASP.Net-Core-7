using MediatR;
using Microsoft.AspNetCore.Http;

namespace e_trade_api.application;

public class UploadProductImageCommandRequest : IRequest<UploadProductImageCommandResponse>
{
    public string Id { get; set; }
    public IFormFileCollection? Files { get; set; } // asp.net.http bağımlılığı oluştu
}
