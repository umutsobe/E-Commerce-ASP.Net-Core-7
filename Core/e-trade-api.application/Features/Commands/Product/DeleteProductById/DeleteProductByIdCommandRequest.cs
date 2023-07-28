using MediatR;

namespace e_trade_api.application;

public class DeleteProductByIdCommandRequest : IRequest<DeleteProductByIdCommandResponse>
{
    public string Id { get; set; }
}
