using e_trade_api.application;
using e_trade_api.domain;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.Persistence;

public class ImageFileService : IImageFileService
{
    IStorageService _storageService;
    IImageFileReadRepository _imageFileReadRepository;
    IImageFileWriteRepository _imageFileWriteRepository;
    readonly ICloudflareService _cloudflareService;

    public ImageFileService(
        IStorageService storageService,
        IImageFileReadRepository imageFileReadRepository,
        IImageFileWriteRepository imageFileWriteRepository,
        ICloudflareService cloudflareService
    )
    {
        _storageService = storageService;
        _imageFileReadRepository = imageFileReadRepository;
        _imageFileWriteRepository = imageFileWriteRepository;
        _cloudflareService = cloudflareService;
    }

    public async Task DeleteImage(string imageId)
    {
        ImageFile imageFile = await _imageFileReadRepository.GetByIdAsync(imageId);
        if (imageFile == null)
            return;

        await _imageFileWriteRepository.RemoveAsync(imageId);
        await _imageFileWriteRepository.SaveAsync();

        await _storageService.DeleteAsync("other", imageFile.FileName);
        await _cloudflareService.PurgeEverythingCache();
    }

    public Task<GetImageByIdResponse> GetImageById(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<GetImageByIdResponse>> GetImagesByDefinition(string definition)
    {
        List<ImageFile> imageFiles = await _imageFileReadRepository.Table
            .Where(i => i.Definition == definition)
            .OrderBy(i => i.Order)
            .ToListAsync();

        List<GetImageByIdResponse> responseImages = new();

        foreach (var image in imageFiles)
        {
            GetImageByIdResponse responseImage =
                new() { Id = image.Id.ToString(), Path = image.Path };
            responseImages.Add(responseImage);
        }

        return responseImages;
    }

    public async Task UploadImage(UploadImageRequestDTO model)
    {
        if (model.Definition == null || model.Files == null)
            return;

        List<StorageFile> datas = await _storageService.UploadImageAsync(
            new()
            {
                ContainerName = "other",
                Definition = model.Definition,
                Files = model.Files
            }
        );

        List<ImageFile> imageFiles = datas
            .Select(
                d =>
                    new ImageFile
                    {
                        Id = Guid.NewGuid(),
                        Definition = d.Definition,
                        FileName = d.FileName,
                        Storage = _storageService.StorageName,
                        Path = d.Path,
                    }
            )
            .ToList();

        await _imageFileWriteRepository.AddRangeAsync(imageFiles);
        await _imageFileWriteRepository.SaveAsync();
        await _cloudflareService.PurgeEverythingCache();
    }

    public async Task UpdateOrderDefinitionImages(UpdateOrderDefinitionImagesRequestDTO model)
    {
        List<ImageFile> imageFiles = await _imageFileReadRepository.Table
            .Where(i => i.Definition == model.Definition)
            .ToListAsync();

        foreach (var image in model.Images)
        {
            var imageToUpdate = imageFiles.FirstOrDefault(i => i.Id.ToString() == image.ImageId);
            if (imageToUpdate != null)
                imageToUpdate.Order = image.Order;
        }

        await _imageFileWriteRepository.SaveAsync();
        await _cloudflareService.PurgeEverythingCache();
    }
}
