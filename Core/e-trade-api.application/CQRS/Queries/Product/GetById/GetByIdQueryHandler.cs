using e_trade_api.domain.Entities;
using MediatR;

namespace e_trade_api.application;

public class GetByIdQueryHandler : IRequestHandler<GetByIdQueryRequest, GetByIdQueryResponse>
{
    readonly IProductService _productService;

    public GetByIdQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<GetByIdQueryResponse> Handle(
        GetByIdQueryRequest request,
        CancellationToken cancellationToken
    )
    {
        GetProductByIdDTO productDTO = await _productService.GetProductById(request.Id);

        return new()
        {
            Name = productDTO.Name,
            Price = productDTO.Price,
            Stock = productDTO.Stock,
        };
    }
}
