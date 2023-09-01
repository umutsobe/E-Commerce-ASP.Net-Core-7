using MediatR;

namespace e_trade_api.application;

public class DeleteProductImageCommandRequest : IRequest<DeleteProductImageCommandResponse>
{
    public string ProductId { get; set; }
    public string? ImageId { get; set; }
}
