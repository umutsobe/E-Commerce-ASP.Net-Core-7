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

    public AzureStorage(IConfiguration configuration)
    {
        _blobServiceClient = new(configuration["Storage:Azure"]);
    }

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

    public bool HasFile(string containerName, string fileName)
    {
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        return _blobContainerClient.GetBlobs().Any(b => b.Name == fileName);
    }

    public async Task<List<(string fileName, string path)>> UploadAsync(
        string containerName,
        IFormFileCollection files
    )
    {
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName); // hesaptan container alınır. servis sadece burada kullanıldı. gerisi containerclient'ın işi.

        await _blobContainerClient.CreateIfNotExistsAsync(); // o isimde container yoksa yeni oluşturulur
        await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer); //containera genel erişim sağlanır

        List<(string fileName, string path)> datas = new(); // yüklenecek dosya ayrıntılarını görmek için liste oluşturuldu

        foreach (IFormFile file in files) //client'tan gelen dosyalar üzerinde geziniyoruz
        {
            BlobClient blobClient = _blobContainerClient.GetBlobClient(
                await FileRenameAsync(file.Name)
            ); // her döngüde blobclient oluşturulacak. blobclient bizim yükleyeceğimiz tekil dosyayı temsil ediyor. senkron şekilde dosya ismini verdik. çünkü isim lazım.

            await blobClient.UploadAsync(file.OpenReadStream()); //bir üst satırda oluşturduğumuz her döngüde yeni oluşturulan blobclient ile dosyayı azure'a gönderiyoruz.

            datas.Add((file.Name, containerName)); // dosyayı bizim görmemiz için listeye ismi ve container ismini(dosyayı atmıyoruz) ekliyoruz
        }
        return datas;
    }

    private Task<string> FileRenameAsync(string fileName)
    {
        string extension = Path.GetExtension(fileName);
        string oldName = Path.GetFileNameWithoutExtension(fileName);
        DateTime now = DateTime.Now;
        string dateTime = now.ToString("yyyy-MM-dd-HH-mm-ss-fff-fffffff");

        string newFileName = $"{NameOperation.CharacterRegulatory(oldName)}-{dateTime}{extension}";
        return Task.FromResult(newFileName);
    }
}
