using System.Threading.Tasks;
using MediatR;
using e_trade_api.application;
using Microsoft.EntityFrameworkCore;

public class GetAllProductAdminQueryHandler
    : IRequestHandler<GetAllProductAdminQueryRequest, GetAllProductAdminQueryResponse>
{
    readonly IProductService _productService;

    public GetAllProductAdminQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<GetAllProductAdminQueryResponse> Handle(
        GetAllProductAdminQueryRequest request,
        CancellationToken cancellationToken
    )
    {
        GetAllProductsResponseAdminDTO responseDTO = await _productService.GetAllProductsAdmin(
            new()
            {
                Page = request.Page,
                Size = request.Size,
                CategoryName = request.CategoryName,
                Keyword = request.Keyword,
                MaxPrice = request.MaxPrice,
                MinPrice = request.MinPrice,
                Sort = request.Sort
            }
        );

        return new()
        {
            Products = responseDTO.Products,
            TotalProductCount = responseDTO.TotalProductCount,
        };
    }
}
