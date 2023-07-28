using e_trade_api.domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace e_trade_api.application;

public class GetProductImageQueryHandler : IRequestHandler<GetProductImageQueryRequest, List<GetProductImageQueryResponse>>
{
    IProductReadRepository _productReadRepository;
    IConfiguration _configuration;

    public GetProductImageQueryHandler(IConfiguration configuration, IProductReadRepository productReadRepository)
    {
        _configuration = configuration;
        _productReadRepository = productReadRepository;
    }

    public async Task<List<GetProductImageQueryResponse>> Handle(GetProductImageQueryRequest request, CancellationToken cancellationToken)
    {
        Product? product = await _productReadRepository.Table
            .Include(p => p.ProductImageFiles)
            .FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));

        return product?.ProductImageFiles.Select(
             p =>
                new GetProductImageQueryResponse
                {
                    Path = $"{_configuration["BaseStorageUrl"]}/{p.Path}",
                    FileName = p.FileName,
                    Id = p.Id
                }
            ).ToList();
    }
}
