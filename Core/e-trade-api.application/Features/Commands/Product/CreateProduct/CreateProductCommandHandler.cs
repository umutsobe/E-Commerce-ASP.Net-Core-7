namespace e_trade_api.application;

using System.Threading;
using System.Threading.Tasks;
using e_trade_api.domain.Entities;
using MediatR;

public class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
{
    readonly IProductWriteRepository _productWriteRepository;
    readonly IProductHubServices _productHubServices;

    public CreateProductCommandHandler(
        IProductWriteRepository productWriteRepository,
        IProductHubServices productHubServices
    )
    {
        _productWriteRepository = productWriteRepository;
        _productHubServices = productHubServices;
    }

    public async Task<CreateProductCommandResponse> Handle(
        CreateProductCommandRequest request,
        CancellationToken cancellationToken
    )
    {
        await _productWriteRepository.AddAsync(
            new Product()
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock
            }
        );
        await _productWriteRepository.SaveAsync();
        await _productHubServices.ProductAddedMessageAsync(
            $"{request.Name} isminde bir ürün eklenmiştir!!"
        );
        return new();
    }
}
