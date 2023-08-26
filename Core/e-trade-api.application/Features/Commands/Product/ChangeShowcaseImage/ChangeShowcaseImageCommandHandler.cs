using MediatR;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.application;

public class ChangeShowcaseImageCommandHandler
    : IRequestHandler<ChangeShowcaseImageCommandRequest, ChangeShowcaseImageCommandResponse>
{
    readonly IProductService _productService;

    public ChangeShowcaseImageCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<ChangeShowcaseImageCommandResponse> Handle(
        ChangeShowcaseImageCommandRequest request,
        CancellationToken cancellationToken
    )
    {
        await _productService.ChangeShowcaseImage(
            new() { ImageId = request.ImageId, ProductId = request.ProductId, }
        );

        return new();
    }
}
