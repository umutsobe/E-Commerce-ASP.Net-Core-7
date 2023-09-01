using System.Threading.Tasks;
using MediatR;
using e_trade_api.application;
using Microsoft.EntityFrameworkCore;

public class GetAllProductQueryHandler
    : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
{
    readonly IProductService _productService;

    public GetAllProductQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<GetAllProductQueryResponse> Handle(
        GetAllProductQueryRequest request,
        CancellationToken cancellationToken
    )
    {
        GetAllProductsResponseDTO responseDTO = await _productService.GetAllProducts(
            new() { Page = request.Page, Size = request.Size, }
        );

        return new()
        {
            Products = responseDTO.Products,
            TotalProductCount = responseDTO.TotalProductCount,
        };
    }
}
