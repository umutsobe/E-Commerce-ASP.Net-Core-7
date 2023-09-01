using e_trade_api.domain;
using e_trade_api.domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.application;

public class DeleteProductImageCommandHandler
    : IRequestHandler<DeleteProductImageCommandRequest, DeleteProductImageCommandResponse>
{
    readonly IProductImageService _productImageService;

    public DeleteProductImageCommandHandler(IProductImageService productImageService)
    {
        _productImageService = productImageService;
    }

    public async Task<DeleteProductImageCommandResponse> Handle(
        DeleteProductImageCommandRequest request,
        CancellationToken cancellationToken
    )
    {
        await _productImageService.DeleteProductImage(
            new() { ProductId = request.ProductId, ImageId = request.ImageId, }
        );

        return new();
    }
}
