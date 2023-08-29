using System.Linq;
using e_trade_api.application;
using e_trade_api.domain;
using e_trade_api.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.Persistence;

public class ProductService : IProductService
{
    readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
    readonly IProductReadRepository _productReadRepository;
    readonly IProductWriteRepository _productWriteRepository;
    readonly IProductHubServices _productHubServices;
    readonly ICategoryReadRepository _categoryReadRepository;

    public ProductService(
        IProductImageFileWriteRepository productImageFileWriteRepository,
        IProductReadRepository productReadRepository,
        IProductWriteRepository productWriteRepository,
        IProductHubServices productHubServices,
        ICategoryReadRepository categoryReadRepository
    )
    {
        _productImageFileWriteRepository = productImageFileWriteRepository;
        _productReadRepository = productReadRepository;
        _productWriteRepository = productWriteRepository;
        _productHubServices = productHubServices;
        _categoryReadRepository = categoryReadRepository;
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
        Guid productId = Guid.NewGuid();

        List<Category> categories = _categoryReadRepository.Table
            .Where(c => model.CategoryNames.Contains(c.Name))
            .ToList();

        await _productWriteRepository.AddAsync(
            new Product()
            {
                Id = productId,
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock,
                Description = model.Description,
                isActive = model.isActive,
                Categories = categories
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
            .OrderBy(p => p.Name)
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

    //category
    public async Task AssignCategoryToProduct(AssignCategoryToProductRequestDTO model)
    {
        var product = await _productReadRepository.Table
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == Guid.Parse(model.ProductId));

        if (product != null)
        {
            product.Categories.Clear();

            foreach (var categoryName in model.CategoryNames)
            {
                Category? category = await _categoryReadRepository.Table.FirstOrDefaultAsync(
                    c => c.Name == categoryName
                );

                if (category != null)
                    product.Categories.Add(category);
            }

            await _productWriteRepository.SaveAsync();
        }
        else
        {
            throw new Exception("Product Not Found");
        }
    }

    public async Task<GetAllProductsResponseDTO> GetProductsByCategory(
        GetAllProductByCategoryRequestDTO model
    )
    {
        var query = _categoryReadRepository.Table
            .Where(c => c.Name == model.CategoryName)
            .Select(
                c =>
                    new GetAllProductsResponseDTO
                    {
                        TotalProductCount = c.Products.Count,
                        Products = c.Products
                            .OrderBy(p => p.Name) //  sıralama
                            .Skip(model.Page * model.Size)
                            .Take(model.Size)
                            .Select(
                                p =>
                                    new ProductResponseDTO
                                    {
                                        Id = p.Id.ToString(),
                                        Name = p.Name,
                                        Stock = p.Stock,
                                        Price = p.Price,
                                        CreatedDate = p.CreatedDate,
                                        UpdatedDate = p.UpdatedDate,
                                        ProductImageFiles = p.ProductImageFiles, // Gerekirse bu kısmı doldurun
                                    }
                            )
                            .ToList()
                    }
            );

        var result = await query.FirstOrDefaultAsync();

        if (result != null)
        {
            return result;
        }
        else
        {
            throw new Exception("Kategori bulunamadı.");
        }
    }

    public async Task<List<string>> GetCategoriesByProduct(string productId)
    {
        var product = _productReadRepository.Table
            .Include(p => p.Categories)
            .FirstOrDefault(p => p.Id == Guid.Parse(productId));

        if (product != null)
        {
            var categoriesForProduct = product.Categories.Select(c => c.Name).ToList();

            return categoriesForProduct;
        }
        else
        {
            throw new Exception("Ürün bulunamadı.");
        }
    }

    public async Task AddProductsToCategory(AddProductsToCategoryRequestDTO model) //ürün daha önce eklenmişse ürünü eklemiyor. sadece eklenmemiş ürünleri ekliyor
    {
        var category = await _categoryReadRepository.Table
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == Guid.Parse(model.CategoryId));

        if (category != null)
        {
            var productIds = model.Products.Select(p => Guid.Parse(p.ProductId)).ToList();

            var productsToAdd = await _productReadRepository.Table
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            foreach (var product in productsToAdd)
            {
                category.Products.Add(product);
            }

            await _productWriteRepository.SaveAsync();
        }
        else
        {
            throw new Exception("Kategori bulunamadı.");
        }
    }
}
