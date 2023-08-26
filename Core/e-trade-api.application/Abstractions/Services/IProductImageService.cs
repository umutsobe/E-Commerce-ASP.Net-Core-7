namespace e_trade_api.application;

public interface IProductImageService
{
    Task DeleteProductImage(ProductImageDeleteRequestDTO model);
    Task UploadProductImage(UploadProductImageRequestDTO model);
    Task<List<GetProductImageQueryResponse>> GetProductImages(string Id);
}
