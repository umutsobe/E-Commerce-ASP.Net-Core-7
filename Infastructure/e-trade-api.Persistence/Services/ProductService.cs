using e_trade_api.application;
using e_trade_api.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.Persistence;

public class ProductService : IProductService
{
    readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
    readonly IProductReadRepository _productReadRepository;
    readonly IProductWriteRepository _productWriteRepository;
    readonly IProductHubServices _productHubServices;

    public ProductService(
        IProductImageFileWriteRepository productImageFileWriteRepository,
        IProductReadRepository productReadRepository,
        IProductWriteRepository productWriteRepository,
        IProductHubServices productHubServices
    )
    {
        _productImageFileWriteRepository = productImageFileWriteRepository;
        _productReadRepository = productReadRepository;
        _productWriteRepository = productWriteRepository;
        _productHubServices = productHubServices;
    }

    //commands

    public async Task ChangeShowcaseImage(ChangeShowCaseImageRequestDTO model)
    {
        var query = _productImageFileWriteRepository.Table
            .Include(p => p.Products)
            .SelectMany(p => p.Products, (pif, p) => new { pif, p });

        var data = await query.FirstOrDefaultAsync(
            p => p.p.Id == Guid.Parse(model.ProductId) && p.pif.Showcase
        );

        if (data != null)
            data.pif.Showcase = false;

        var image = await query.FirstOrDefaultAsync(p => p.pif.Id == Guid.Parse(model.ImageId));
        if (image != null)
            image.pif.Showcase = true;

        await _productImageFileWriteRepository.SaveAsync();
    }

    public async Task CreateProduct(CreateProductDTO model)
    {
        await _productWriteRepository.AddAsync(
            new Product()
            {
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock
            }
        );

        await _productWriteRepository.SaveAsync();

        await _productHubServices.ProductAddedMessageAsync(
            $"{model.Name} isminde bir ürün eklenmiştir!!"
        );
    }

    public async Task DeleteProductById(string Id)
    {
        await _productWriteRepository.RemoveAsync(Id);
        await _productWriteRepository.SaveAsync();
    }

    public async Task UpdateProduct(UpdateProductDTO model)
    {
        Product product = await _productReadRepository.GetByIdAsync(model.Id);
        product.Name = model.Name;
        product.Price = model.Price;
        product.Stock = model.Stock;

        await _productWriteRepository.SaveAsync();
    }

    //queries
    public Task<GetAllProductsResponseDTO> GetAllProducts(GetAllProductRequestDTO model)
    {
        var totalProductCount = _productReadRepository.GetAll(false).Where(p => p.isActive).Count();

        var databaseProducts = _productReadRepository
            .GetAll(false)
            .Where(p => p.isActive)
            .Skip(model.Page * model.Size)
            .Take(model.Size)
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

        GetAllProductsResponseDTO productsDTO = new GetAllProductsResponseDTO();
        productsDTO.Products = new List<ProductResponseDTO>(); // !!!! aşağıda buna eleman ekliyoruz. null hatası almamak için sınıfın instance'ını oluşturduk

        if (databaseProducts != null)
        {
            foreach (var product in databaseProducts)
            {
                if (product != null)
                {
                    ProductResponseDTO productDTO =
                        new()
                        {
                            Id = product.Id.ToString(),
                            Name = product.Name,
                            Stock = product.Stock,
                            Price = product.Price,
                            CreatedDate = product.CreatedDate,
                            UpdatedDate = product.UpdatedDate,
                            ProductImageFiles = product.ProductImageFiles,
                        };

                    productsDTO.Products.Add(productDTO);
                }
            }

            productsDTO = new()
            {
                TotalProductCount = totalProductCount,
                Products = productsDTO.Products
            };
        }

        return Task.FromResult(productsDTO);
    }

    public async Task<GetProductByIdDTO> GetProductById(string Id)
    {
        Product product = await _productReadRepository.GetByIdAsync(Id, false);

        return new()
        {
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock,
        };
    }
}
