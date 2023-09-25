namespace e_trade_api.application;

public interface IImageFileService
{
    Task DeleteImage(string imageId);
    Task UploadImage(UploadImageRequestDTO model);
    Task<GetImageByIdResponse> GetImageById(string id);
    Task<List<GetImageByIdResponse>> GetImagesByDefinition(string definition);
    Task UpdateOrderDefinitionImages(UpdateOrderDefinitionImagesRequestDTO model);
}
