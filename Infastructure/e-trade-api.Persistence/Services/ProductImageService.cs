using e_trade_api.application;
using e_trade_api.domain;
using e_trade_api.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.Persistence;

public class ProductImageService : IProductImageService
{
    IProductReadRepository _productReadRepository;
    IProductImageFileWriteRepository _productImageFileWriteRepository;
    IStorageService _storageService;

    public ProductImageService(
        IStorageService storageService,
        IProductImageFileWriteRepository productImageFileWriteRepository,
        IProductReadRepository productReadRepository
    )
    {
        _storageService = storageService;
        _productImageFileWriteRepository = productImageFileWriteRepository;
        _productReadRepository = productReadRepository;
    }

    public async Task DeleteProductImage(ProductImageDeleteRequestDTO model)
    {
        Product? product = await _productReadRepository.Table
            .Include(p => p.ProductImageFiles)
            .FirstOrDefaultAsync(p => p.Id == Guid.Parse(model.ProductId));

        ProductImageFile? productImageFile = product?.ProductImageFiles.FirstOrDefault(
            p => p.Id == Guid.Parse(model.ImageId)
        );

        await _productImageFileWriteRepository.RemoveAsync(productImageFile.Id.ToString());
        await _productImageFileWriteRepository.SaveAsync();

        string imageName = productImageFile.Path.Split("/")[1]; // bize / işaretinin gelme ihtimali yok çünkü characteregulatory ile / işaretini boş stringe dönüştürmüştük

        await _storageService.DeleteAsync("product-image", imageName); // fotoğrafı azuredan da sildik
    }

    public async Task UploadProductImage(UploadProductImageRequestDTO model)
    {
        Product product = await _productReadRepository.GetByIdAsync(model.Id);

        List<StorageFile> datas = await _storageService.UploadAsync(
            new()
            {
                ContainerName = "product-image",
                Files = model.Files,
                ProductName = product.Name
            }
        );

        List<ProductImageFile> imageFiles = datas
            .Select(
                d =>
                    new ProductImageFile()
                    {
                        FileName = d.FileName,
                        Path = d.Path,
                        Storage = _storageService.StorageName,
                        Product = product,
                        Showcase = product.ProductImageFiles == null // İlk dosya için sadece bu true, diğerleri false
                    }
            )
            .ToList();

        if (imageFiles.Count > 0)
        {
            imageFiles[0].Showcase = true;
        }

        await _productImageFileWriteRepository.AddRangeAsync(imageFiles);
        await _productImageFileWriteRepository.SaveAsync();
    }

    public async Task<List<GetProductImageQueryResponse>> GetProductImages(string Id)
    {
        Product? product = await _productReadRepository.Table
            .Include(p => p.ProductImageFiles)
            .FirstOrDefaultAsync(p => p.Id == Guid.Parse(Id));

        List<GetProductImageQueryResponse>? responses = product?.ProductImageFiles
            .Select(
                p =>
                    new GetProductImageQueryResponse
                    {
                        Path = $"{MyConfigurationManager.GetBaseAzureStorageUrl()}/{p.Path}",
                        FileName = p.FileName,
                        Id = p.Id.ToString()
                    }
            )
            .ToList();

        return responses;
    }
}
