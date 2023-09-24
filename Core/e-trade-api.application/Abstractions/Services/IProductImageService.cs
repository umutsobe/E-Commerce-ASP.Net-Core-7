namespace e_trade_api.application;

public interface IProductImageService
{
    Task DeleteProductImage(ProductImageDeleteRequestDTO model);
    Task UploadProductImage(UploadImageRequestDTO model);
    Task<List<GetProductImageQueryResponse>> GetProductImages(string Id);

    Task<GetProductShowcaseImageResponse> GetProductShowcaseImage(string productId);
}
