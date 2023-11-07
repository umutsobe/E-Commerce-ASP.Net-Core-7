using e_trade_api.domain.Entities;
using MediatR;

namespace e_trade_api.application;

public class UpdateProductCommandHandler
    : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
{
    IProductReadRepository _productReadRepository;
    IProductWriteRepository _productWriteRepository;

    public UpdateProductCommandHandler(
        IProductReadRepository productReadRepository,
        IProductWriteRepository productWriteRepository
    )
    {
        _productReadRepository = productReadRepository;
        _productWriteRepository = productWriteRepository;
    }

    public async Task<UpdateProductCommandResponse> Handle(
        UpdateProductCommandRequest request,
        CancellationToken cancellationToken
    )
    {
        Product product = await _productReadRepository.GetByIdAsync(request.Id);
        product.Name = request.Name;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.Description = request.Description;

        await _productWriteRepository.SaveAsync();

        return new();
    }
}
