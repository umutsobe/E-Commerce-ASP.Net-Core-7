using e_trade_api.domain.Entities;
using MediatR;

namespace e_trade_api.application;

public class GetByIdQueryHandler : IRequestHandler<GetByIdQueryRequest, GetByIdQueryResponse>
{
    readonly IProductReadRepository _productReadRepository;

    public GetByIdQueryHandler(IProductReadRepository productReadRepository)
    {
        _productReadRepository = productReadRepository;
    }

    public async Task<GetByIdQueryResponse> Handle(GetByIdQueryRequest request, CancellationToken cancellationToken)
    {
        Product product = await _productReadRepository.GetByIdAsync(request.Id, false);
        return new()
        {
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock,
        };
    }
}
