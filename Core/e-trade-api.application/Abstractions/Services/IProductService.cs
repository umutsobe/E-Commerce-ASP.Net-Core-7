namespace e_trade_api.application;

public interface IProductService
{
    //commands
    public Task CreateProduct(CreateProductDTO model);
    public Task ChangeShowcaseImage(ChangeShowCaseImageRequestDTO model);
    public Task DeleteProductById(string Id);
    public Task UpdateProduct(UpdateProductDTO model);

    //queryies

    Task<GetAllProductsResponseDTO> GetAllProducts(GetAllProductRequestDTO model);
    Task<GetProductByIdDTO> GetProductById(string Id);
    Task AssignCategoryToProduct(AssignCategoryToProductRequestDTO model);

    Task<List<string>> GetCategoriesByProduct(string productId);
    Task AddProductsToCategory(AddProductsToCategoryRequestDTO model);
    Task<GetAllProductsResponseDTO> GetProductsByFilter(GetProductsByFilterDTO model);
    Task<GetAllProductsResponseDTO> GetProductsBySearch(GetProductsBySearchRequestDTO model);
}
