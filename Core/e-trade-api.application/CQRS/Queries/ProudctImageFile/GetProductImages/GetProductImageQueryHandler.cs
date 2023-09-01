using MediatR;

namespace e_trade_api.application;

public class GetProductImageQueryHandler
    : IRequestHandler<GetProductImageQueryRequest, List<GetProductImageQueryResponse>>
{
    IProductImageService _productImageService;

    public GetProductImageQueryHandler(IProductImageService productImageService)
    {
        _productImageService = productImageService;
    }

    public async Task<List<GetProductImageQueryResponse>> Handle(
        GetProductImageQueryRequest request,
        CancellationToken cancellationToken
    )
    {
        return await _productImageService.GetProductImages(request.Id);
    }
}
