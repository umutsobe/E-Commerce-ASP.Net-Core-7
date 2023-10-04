using e_trade_api.application;
using e_trade_api.domain;
using e_trade_api.domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace e_trade_api.Persistence;

public class ProductService : IProductService
{
    readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
    readonly IProductImageFileReadRepository _productImageFileReadRepository;
    readonly IProductReadRepository _productReadRepository;
    readonly IProductWriteRepository _productWriteRepository;
    readonly IProductHubServices _productHubServices;
    readonly ICategoryReadRepository _categoryReadRepository;
    readonly IConfiguration _configuration;

    public ProductService(
        IProductImageFileWriteRepository productImageFileWriteRepository,
        IProductReadRepository productReadRepository,
        IProductWriteRepository productWriteRepository,
        IProductHubServices productHubServices,
        ICategoryReadRepository categoryReadRepository,
        IProductImageFileReadRepository productImageFileReadRepository,
        IConfiguration configuration
    )
    {
        _productImageFileWriteRepository = productImageFileWriteRepository;
        _productReadRepository = productReadRepository;
        _productWriteRepository = productWriteRepository;
        _productHubServices = productHubServices;
        _categoryReadRepository = categoryReadRepository;
        _productImageFileReadRepository = productImageFileReadRepository;
        _configuration = configuration;
    }

    public async Task ChangeShowcaseImage(ChangeShowCaseImageRequestDTO model)
    {
        List<ProductImageFile> productImageFiles = _productImageFileReadRepository.Table
            .Where(pif => pif.ProductId == Guid.Parse(model.ProductId))
            .ToList();

        foreach (var item in productImageFiles)
        {
            item.Showcase = false;
        }

        await _productImageFileWriteRepository.SaveAsync();

        ProductImageFile productImageFile = await _productImageFileReadRepository.GetByIdAsync(
            model.ImageId
        );
        productImageFile.Showcase = true;

        await _productImageFileWriteRepository.SaveAsync();
    }

    public async Task CreateProduct(CreateProductDTO model)
    {
        Guid productId = Guid.NewGuid();
        string productUrl = UrlBuilder.ProductUrlBuilder(model.Name).ToLower();

        List<Category> categories = _categoryReadRepository.Table
            .Where(c => model.CategoryNames.Contains(c.Name))
            .ToList();

        await _productWriteRepository.AddAsync(
            new Product()
            {
                Id = productId,
                Name = model.Name.Trim() + _configuration.GetConnectionString("SQLServer"),
                Price = model.Price,
                Stock = model.Stock,
                Description = model.Description.Trim(),
                isActive = model.isActive,
                Url = productUrl,
                Categories = categories
            }
        );

        await _productWriteRepository.SaveAsync();

        await _productHubServices.ProductAddedMessageAsync(
            $"A product named {model.Name} has been added"
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

        List<Category> categories = _categoryReadRepository.Table
            .Where(c => model.CategoryNames.Contains(c.Name))
            .ToList();

        product.Name = model.Name;
        product.Price = model.Price;
        product.Stock = model.Stock;
        product.Description = model.Description;
        product.isActive = model.IsActive;
        product.Categories = categories;

        await _productWriteRepository.SaveAsync();
    }

    public async Task<GetAllProductsResponseAdminDTO> GetAllProductsAdmin(
        GetProductsByFilterDTO model
    )
    {
        model.Size = 8;
        var query = _productReadRepository.Table
            .Include(p => p.Categories)
            .Include(p => p.ProductImageFiles)
            .AsQueryable();

        if (!string.IsNullOrEmpty(model.Keyword))
            query = query.Where(p => EF.Functions.Like(p.Name, $"%{model.Keyword}%"));

        if (!string.IsNullOrEmpty(model.CategoryName))
            query = query.Where(p => p.Categories.Any(c => c.Name == model.CategoryName));

        if (model.MinPrice.HasValue)
            query = query.Where(p => p.Price >= model.MinPrice);

        if (model.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= model.MaxPrice);

        int totalProductCount = await query.CountAsync(); //skip take yapmadan önce count yaptık

        if (!string.IsNullOrEmpty(model.Sort))
        {
            if (model.Sort == "desc")
                query = query.OrderByDescending(p => p.Price);
            else if (model.Sort == "asc")
                query = query.OrderBy(p => p.Price);
            else if (model.Sort == "sales")
                query = query.OrderByDescending(p => p.TotalOrderNumber);
        }
        else
            query = query.OrderBy(p => p.CreatedDate);

        query = query.Skip(model.Page * model.Size).Take(model.Size);

        var filteredProducts = await query
            .Select(
                p =>
                    new ProductResponseAdminDTO
                    {
                        Id = p.Id.ToString(),
                        Name = p.Name,
                        Stock = p.Stock,
                        Price = p.Price,
                        Url = p.Url,
                        CreatedDate = p.CreatedDate,
                        UpdatedDate = p.UpdatedDate,
                        ProductImageFiles = p.ProductImageFiles
                            .Select(
                                pif =>
                                    new ProductImageFileResponseDTO
                                    {
                                        FileName = pif.FileName,
                                        Id = pif.Id.ToString(),
                                        Path = pif.Path,
                                        Showcase = pif.Showcase
                                    }
                            )
                            .ToList(),
                        IsActive = p.isActive,
                        Description = p.Description,
                        TotalBasketAdded = p.TotalBasketAdded,
                        TotalOrderNumber = p.TotalOrderNumber
                    }
            )
            .ToListAsync();

        return new() { Products = filteredProducts, TotalProductCount = totalProductCount };
    }

    public async Task<GetProductByIdDTO> GetProductByUrlId(string urlId)
    {
        Product? product = await _productReadRepository.Table
            .Include(p => p.ProductImageFiles)
            .Include(p => p.ProductRatings)
            .Where(p => p.Url == urlId)
            .FirstOrDefaultAsync();

        if (product != null)
        {
            return new()
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Url = product.Url,
                ProductImageFiles = product.ProductImageFiles
                    .Select(
                        pif =>
                            new ProductImageFileResponseDTO
                            {
                                Id = pif.Id.ToString(),
                                FileName = pif.FileName,
                                Showcase = pif.Showcase,
                                Path = pif.Path
                            }
                    )
                    .ToList(),
                TotalRatingNumber = product.ProductRatings.Count,
                AverageStar =
                    product.ProductRatings.Count == 0
                        ? 0
                        : (double)product.ProductRatings.Sum(pr => pr.Star)
                            / product.ProductRatings.Count
            };
        }
        else
            throw new Exception("Product Not Found");
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

    public async Task<GetAllProductsResponseDTO> GetProductsByFilter(GetProductsByFilterDTO model) //kullanıcı için
    {
        var query = _productReadRepository.Table.Include(p => p.Categories).AsQueryable();

        query = query.Where(p => p.isActive && p.Stock > 0); //sadece aktifler ve stoğu olanlar gelecek

        if (!string.IsNullOrEmpty(model.Keyword))
            query = query.Where(p => EF.Functions.Like(p.Name, $"%{model.Keyword}%"));

        if (!string.IsNullOrEmpty(model.CategoryName))
            query = query.Where(
                p => p.Categories.Any(c => c.Name.ToLower() == model.CategoryName.ToLower())
            );

        if (model.MinPrice.HasValue)
            query = query.Where(p => p.Price >= model.MinPrice);

        if (model.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= model.MaxPrice);

        int totalProductCount = await query.CountAsync(); //skip take yapmadan önce count yaptık

        if (!string.IsNullOrEmpty(model.Sort))
        {
            if (model.Sort == "desc")
                query = query.OrderByDescending(p => p.Price);
            else if (model.Sort == "asc")
                query = query.OrderBy(p => p.Price);
            else if (model.Sort == "sales")
                query = query.OrderByDescending(p => p.TotalOrderNumber);
        }
        else
            query = query.OrderBy(p => p.CreatedDate);

        query = query.Skip(model.Page * model.Size).Take(model.Size);

        return new()
        {
            Products = await query
                .Select(
                    p =>
                        new ProductResponseDTO
                        {
                            Id = p.Id.ToString(),
                            Name = p.Name,
                            Stock = p.Stock,
                            Price = p.Price,
                            Url = p.Url,
                            ProductImageShowCasePath = p.ProductImageFiles
                                .FirstOrDefault(
                                    pif => pif.ProductId == p.Id && pif.Showcase == true
                                )
                                .Path,
                            TotalRatingNumber = p.ProductRatings.Count,
                            AverageStar =
                                p.ProductRatings.Count == 0
                                    ? 0
                                    : (double)p.ProductRatings.Sum(pr => pr.Star)
                                        / p.ProductRatings.Count
                        }
                )
                .ToListAsync(),
            TotalProductCount = totalProductCount
        };
    }

    public async Task QuickCreateProduct()
    {
        Random random = new();
        List<Product> products = new();

        for (int i = 0; i < 120; i++)
        {
            Guid productId = Guid.NewGuid();
            string name = $"Product {i + 1}";
            float price = 10 * (i + 1);
            int stock = 10 * (i + 1);
            string description = $"<h1>{name} description</h1>";
            int totalBasketAdded = random.Next(0, 100);
            int totalOrderNumber = random.Next(0, 100);

            string productUrl = UrlBuilder.ProductUrlBuilder(name).ToLower();

            products.Add(
                new()
                {
                    Id = productId,
                    Name = name.Trim(),
                    Price = price,
                    Stock = stock,
                    Description = description.Trim(),
                    isActive = true,
                    Url = productUrl,
                    TotalBasketAdded = totalBasketAdded,
                    TotalOrderNumber = totalOrderNumber,
                }
            );
        }

        await _productWriteRepository.AddRangeAsync(products);
        await _productWriteRepository.SaveAsync();
    }

    public async Task AddStock(AddStockRequestDTO model)
    {
        Product product = await _productReadRepository.GetByIdAsync(model.ProductId);
        product.Stock += model.StockToBeAdded;

        await _productWriteRepository.SaveAsync();
    }

    public async Task DeactivateProduct(string productId)
    {
        Product product = await _productReadRepository.GetByIdAsync(productId);
        product.isActive = false;

        await _productWriteRepository.SaveAsync();
    }

    public async Task ActivateProduct(string productId)
    {
        Product product = await _productReadRepository.GetByIdAsync(productId);
        product.isActive = true;

        await _productWriteRepository.SaveAsync();
    }
}
