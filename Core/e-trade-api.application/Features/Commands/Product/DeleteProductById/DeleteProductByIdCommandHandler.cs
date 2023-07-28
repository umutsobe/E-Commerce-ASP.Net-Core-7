using MediatR;

namespace e_trade_api.application;

public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommandRequest, DeleteProductByIdCommandResponse>
{
    readonly IProductWriteRepository _productWriteRepository;

    public DeleteProductByIdCommandHandler(IProductWriteRepository productWriteRepository)
    {
        _productWriteRepository = productWriteRepository;
    }

    public async Task<DeleteProductByIdCommandResponse> Handle(DeleteProductByIdCommandRequest request, CancellationToken cancellationToken)
    {
        await _productWriteRepository.RemoveAsync(request.Id);
        await _productWriteRepository.SaveAsync();

        return new();
    }
}
