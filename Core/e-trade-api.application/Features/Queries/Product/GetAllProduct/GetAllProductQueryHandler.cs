using System.Threading.Tasks;
using MediatR;
using e_trade_api.application;
using Microsoft.EntityFrameworkCore;

public class GetAllProductQueryHandler
    : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
{
    readonly IProductReadRepository _productReadRepository;

    public GetAllProductQueryHandler(IProductReadRepository productReadRepository)
    {
        _productReadRepository = productReadRepository;
    }

    public Task<GetAllProductQueryResponse> Handle(
        GetAllProductQueryRequest request,
        CancellationToken cancellationToken
    )
    {
        var totalProductCount = _productReadRepository.GetAll(false).Count();

        var products = _productReadRepository
            .GetAll(false)
            .Skip(request.Page * request.Size)
            .Take(request.Size)
            .Include(p => p.ProductImageFiles)
            .Select(
                p =>
                    new
                    {
                        p.Id,
                        p.Name,
                        p.Stock,
                        p.Price,
                        p.CreatedDate,
                        p.UpdatedDate,
                        p.ProductImageFiles
                    }
            )
            .ToList();

        GetAllProductQueryResponse response =
            new() { TotalProductCount = totalProductCount, Products = products };

        return Task.FromResult(response);
    }
}
