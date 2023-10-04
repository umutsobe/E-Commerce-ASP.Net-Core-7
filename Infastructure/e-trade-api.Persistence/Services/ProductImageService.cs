using e_trade_api.application;
using e_trade_api.domain;
using e_trade_api.domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace e_trade_api.Persistence;

public class ProductImageService : IProductImageService
{
    IProductReadRepository _productReadRepository;
    IProductImageFileReadRepository _productImageFileReadRepository;
    IProductImageFileWriteRepository _productImageFileWriteRepository;
    IStorageService _storageService;
    readonly IConfiguration _configuration;

    public ProductImageService(
        IStorageService storageService,
        IProductImageFileWriteRepository productImageFileWriteRepository,
        IProductReadRepository productReadRepository,
        IProductImageFileReadRepository productImageFileReadRepository,
        IConfiguration configuration
    )
    {
        _storageService = storageService;
        _productImageFileWriteRepository = productImageFileWriteRepository;
        _productReadRepository = productReadRepository;
        _productImageFileReadRepository = productImageFileReadRepository;
        _configuration = configuration;
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

    public async Task UploadProductImage(UploadImageRequestDTO model)
    {
        Product product = await _productReadRepository.GetByIdAsync(model.Id);

        List<StorageFile> datas = await _storageService.UploadProductImageAsync(
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
                    }
            )
            .ToList();

        await _productImageFileWriteRepository.AddRangeAsync(imageFiles);
        await _productImageFileWriteRepository.SaveAsync();

        List<ProductImageFile> productImageFiles = _productImageFileReadRepository.Table
            .Where(pif => pif.ProductId == product.Id)
            .ToList();

        foreach (var item in productImageFiles)
        {
            item.Showcase = false;
        }

        productImageFiles[0].Showcase = true;
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
                        Id = p.Id.ToString(),
                        Path = $"{_configuration.GetValue<string>("BaseStorageUrl")}/{p.Path}",
                        Showcase = p.Showcase
                    }
            )
            .ToList();

        return responses;
    }

    public async Task<GetProductShowcaseImageResponse> GetProductShowcaseImage(string productId)
    {
        ProductImageFile? productImageFile = await _productImageFileReadRepository.Table
            .Where(pif => pif.ProductId == Guid.Parse(productId) && pif.Showcase == true)
            .FirstOrDefaultAsync();

        if (productImageFile != null)
            return new() { Path = productImageFile.Path };

        return new();
    }
}
