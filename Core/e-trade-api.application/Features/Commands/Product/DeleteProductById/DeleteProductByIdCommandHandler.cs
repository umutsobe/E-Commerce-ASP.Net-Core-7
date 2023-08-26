using MediatR;

namespace e_trade_api.application;

public class DeleteProductByIdCommandHandler
    : IRequestHandler<DeleteProductByIdCommandRequest, DeleteProductByIdCommandResponse>
{
    readonly IProductService _productService;

    public DeleteProductByIdCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<DeleteProductByIdCommandResponse> Handle(
        DeleteProductByIdCommandRequest request,
        CancellationToken cancellationToken
    )
    {
        await _productService.DeleteProductById(request.Id);

        return new();
    }
}
