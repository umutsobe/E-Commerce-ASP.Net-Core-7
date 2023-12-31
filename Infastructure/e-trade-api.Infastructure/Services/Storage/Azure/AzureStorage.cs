using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using e_trade_api.application;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace e_trade_api.Infastructure;

public class AzureStorage : IAzureStorage
{
    readonly BlobServiceClient _blobServiceClient; //hesaba bağlanırken
    BlobContainerClient _blobContainerClient; // hesapta dosya işlemleri için
    private readonly IConfiguration _configuration;

    public AzureStorage(IConfiguration configuration)
    {
        _configuration = configuration;
        _blobServiceClient = new(_configuration.GetValue<string>("Storage:Azure"));
    }

    public string StorageName => GetType().Name;

    public async Task DeleteAsync(string containerName, string fileName)
    {
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);

        await blobClient.DeleteAsync();
    }

    public List<string> GetFiles(string containerName)
    {
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        return _blobContainerClient.GetBlobs().Select(b => b.Name).ToList();
    }

    public async Task<bool> HasFile(string containerName, string fileName)
    {
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await foreach (BlobItem blobItem in _blobContainerClient.GetBlobsAsync())
        {
            if (blobItem.Name == fileName)
            {
                return true;
            }
        }
        return false;
    }

    public async Task<List<StorageFile>> UploadProductImageAsync(UploadProductImageRequest model)
    {
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(model.ContainerName); // hesaptan container alınır. servis sadece burada kullanıldı. gerisi containerclient'ın işi.

        await _blobContainerClient.CreateIfNotExistsAsync(); // o isimde container yoksa yeni oluşturulur
        await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer); //containera genel erişim sağlanır

        List<StorageFile> datas = new(); // yüklenecek dosya ayrıntılarını veritabanına eklemek için liste oluşturuldu

        foreach (IFormFile file in model.Files)
        {
            string extension = ".webp";

            string productFileName =
                $"{model.ProductName}-{Guid.NewGuid().ToString().Substring(0, 7)}{extension}"; //5 haneli guid productName yanına geldi

            Stream fileStream = CompressImage.Compress( //image compression. bütün dosyalar .webp'ye dönüştürülüyor
                file.OpenReadStream(),
                productFileName,
                70,
                800,
                800
            );

            fileStream.Position = 0;

            BlobClient blobClient = _blobContainerClient.GetBlobClient(productFileName);

            await blobClient.UploadAsync(fileStream);

            datas.Add(
                new()
                {
                    FileName = productFileName,
                    Path = $"{model.ContainerName}/{productFileName}"
                }
            ); //veritabanına atmak için
        }
        return datas;
    }

    public async Task<List<StorageFile>> UploadImageAsync(ImageFileRequest model)
    {
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(model.ContainerName);

        await _blobContainerClient.CreateIfNotExistsAsync();
        await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

        List<StorageFile> datas = new();

        foreach (IFormFile file in model.Files)
        {
            string extension = Path.GetExtension(file.Name);
            string productFileName =
                $"{model.Definition}-{Guid.NewGuid().ToString().Substring(0, 8)}{extension}";

            Stream fileStream = CompressImage.Compress( //image compression. bütün dosyalar .webp'ye dönüştürülüyor
                file.OpenReadStream(),
                productFileName,
                70,
                1600,
                800
            );

            fileStream.Position = 0;

            BlobClient blobClient = _blobContainerClient.GetBlobClient(productFileName);

            await blobClient.UploadAsync(fileStream);

            datas.Add(
                new()
                {
                    Definition = model.Definition,
                    FileName = productFileName,
                    Path = $"{model.ContainerName}/{productFileName}"
                }
            );
        }

        return datas;
    }
}
