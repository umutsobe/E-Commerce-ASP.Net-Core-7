using e_trade_api.domain;
using e_trade_api.domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.application;

public class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommandRequest, DeleteProductImageCommandResponse>
{
    IProductReadRepository _productReadRepository;
    IProductImageFileWriteRepository _productImageFileWriteRepository;
    IStorageService _storageService;

    public DeleteProductImageCommandHandler(IProductReadRepository productReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IStorageService storageService)
    {
        _productReadRepository = productReadRepository;
        _productImageFileWriteRepository = productImageFileWriteRepository;
        _storageService = storageService;
    }

    public async Task<DeleteProductImageCommandResponse> Handle(DeleteProductImageCommandRequest request, CancellationToken cancellationToken)
    {
        Product? product = await _productReadRepository.Table
            .Include(p => p.ProductImageFiles)
            .FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.ProductId));

        ProductImageFile? productImageFile = product?.ProductImageFiles.FirstOrDefault(
            p => p.Id == Guid.Parse(request.ImageId)
        );

        await _productImageFileWriteRepository.RemoveAsync(productImageFile.Id.ToString());
        await _productImageFileWriteRepository.SaveAsync();

        string imageName = productImageFile.Path.Split("/")[1]; // bize / işaretinin gelme ihtimali yok çünkü characteregulatory ile / işaretini boş stringe dönüştürmüştük

        await _storageService.DeleteAsync("product-image", imageName); // fotoğrafı azuredan da sildik

        return new();
    }
}
