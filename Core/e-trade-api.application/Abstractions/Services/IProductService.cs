namespace e_trade_api.application;

public interface IProductService
{
    //commands
    public Task CreateProduct(CreateProductDTO model);
    public Task ChangeShowcaseImage(ChangeShowCaseImageRequestDTO model);
    public Task DeleteProductById(string Id);
    public Task UpdateProduct(UpdateProductDTO model);

    //queryies

    Task<GetAllProductsResponseAdminDTO> GetAllProductsAdmin(GetProductsByFilterDTO model);
    Task<GetProductByIdDTO> GetProductByUrlId(string urlId);
    Task AssignCategoryToProduct(AssignCategoryToProductRequestDTO model);

    Task<List<string>> GetCategoriesByProduct(string productId);
    Task AddProductsToCategory(AddProductsToCategoryRequestDTO model);
    Task<GetAllProductsResponseDTO> GetProductsByFilter(GetProductsByFilterDTO model);

    //develeopment

    public Task QuickCreateProduct();
}
