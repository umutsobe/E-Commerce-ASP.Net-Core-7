namespace e_trade_api.application;

using System.Threading;
using System.Threading.Tasks;
using e_trade_api.domain.Entities;
using MediatR;

public class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
{
    readonly IProductService _productService;

    public CreateProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<CreateProductCommandResponse> Handle(
        CreateProductCommandRequest request,
        CancellationToken cancellationToken
    )
    {
        await _productService.CreateProduct(
            new()
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock,
                isActive = request.isActive,
                CategoryNames = request.CategoryNames,
                Description = request.Description,
            }
        );

        return new();
    }
}
