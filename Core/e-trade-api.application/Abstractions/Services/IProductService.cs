namespace e_trade_api.application;

public interface IProductService
{
    //commands
    public Task CreateProduct(CreateProductDTO model);
    public Task ChangeShowcaseImage(ChangeShowCaseImageRequestDTO model);
    public Task DeleteProductById(string Id);
    public Task UpdateProduct(UpdateProductDTO model);

    //queryies

    public Task<GetAllProductsResponseDTO> GetAllProducts(GetAllProductRequestDTO model);
    public Task<GetProductByIdDTO> GetProductById(string Id);
}
