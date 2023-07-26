using System.Net;
using e_trade_api.application;
using e_trade_api.domain;
using e_trade_api.domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.API;

[Route("api/[controller]")]
[ApiController]
public class ProductControllers : ControllerBase
{
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IProductReadRepository _productReadRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    readonly IFileWriteRepository _fileWriteRepository;
    readonly IFileReadRepository _fileReadRepository;
    readonly IProductImageFileReadRepository _productImageFileReadRepository;
    readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
    readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
    readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
    readonly IStorageService _storageService;
    readonly IConfiguration _configuration;

    public ProductControllers( //constructer
        IProductReadRepository productReadRepository,
        IProductWriteRepository productWriteRepository,
        IWebHostEnvironment webHostEnvironment,
        IFileWriteRepository fileWriteRepository,
        IFileReadRepository fileReadRepository,
        IProductImageFileReadRepository productImageFileReadRepository,
        IProductImageFileWriteRepository productImageFileWriteRepository,
        IInvoiceFileReadRepository invoiceFileReadRepository,
        IInvoiceFileWriteRepository invoiceFileWriteRepository,
        IStorageService storageService,
        IConfiguration configuration
    )
    {
        _productReadRepository = productReadRepository;
        _productWriteRepository = productWriteRepository;
        _webHostEnvironment = webHostEnvironment;
        _fileWriteRepository = fileWriteRepository;
        _fileReadRepository = fileReadRepository;
        _productImageFileReadRepository = productImageFileReadRepository;
        _productImageFileWriteRepository = productImageFileWriteRepository;
        _invoiceFileReadRepository = invoiceFileReadRepository;
        _invoiceFileWriteRepository = invoiceFileWriteRepository;
        _storageService = storageService;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Pagination pagination) // eğer api üzerinden bir şey geriye döndürüyorsak iactionresult döndürmek zorundayız
    {
        // await Task.Delay(1500);
        var totalCount = _productReadRepository.GetAll(false).Count();
        var products = _productReadRepository
            .GetAll(false)
            .Skip(pagination.Page * pagination.Size)
            .Take(pagination.Size)
            .Select(
                p =>
                    new
                    {
                        p.Id,
                        p.Name,
                        p.Stock,
                        p.Price,
                        p.CreatedDate,
                        p.UpdatedDate
                    }
            )
            .ToList();

        return Ok(new { totalCount, products });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id) // eğer api üzerinden bir şey geriye döndürüyorsak iactionresult döndürmek zorundayız
    {
        return Ok(await _productReadRepository.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> Post(VM_Create_Product model)
    {
        await _productWriteRepository.AddAsync(
            new Product()
            {
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock
            }
        );
        await _productWriteRepository.SaveAsync();
        return StatusCode(201);
    }

    [HttpPut]
    public async Task<IActionResult> Put(VM_Update_Product model)
    {
        Product product = await _productReadRepository.GetByIdAsync(model.Id);
        product.Name = model.Name;
        product.Price = model.Price;
        product.Stock = model.Stock;

        await _productWriteRepository.SaveAsync();

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _productWriteRepository.RemoveAsync(id);
        await _productWriteRepository.SaveAsync();

        return Ok();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Upload(string id)
    {
        List<(string fileName, string path)> datas = await _storageService.UploadAsync(
            "product-image",
            Request.Form.Files
        );

        Product product = await _productReadRepository.GetByIdAsync(id);

        await _productImageFileWriteRepository.AddRangeAsync(
            datas
                .Select(
                    d =>
                        new ProductImageFile()
                        {
                            FileName = d.fileName,
                            Path = d.path,
                            Storage = _storageService.StorageName,
                            Products = new List<Product>() { product }
                        }
                )
                .ToList()
        );
        await _productImageFileWriteRepository.SaveAsync();
        return Ok();
    }

    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> GetProductImages(string id)
    {
        Product product = await _productReadRepository.Table
            .Include(p => p.ProductImageFiles)
            .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

        return Ok(
            product.ProductImageFiles.Select(
                p =>
                    new
                    {
                        Path = $"{_configuration["BaseStorageUrl"]}/{p.Path}",
                        p.FileName,
                        p.Id
                    }
            )
        );
    }

    [HttpDelete("[action]/{productId}")] //alttaki isimle buradaki isim aynı olmak zorunda. buradaki productId, pareametredeki productId
    public async Task<IActionResult> DeleteImage(string productId, string imageId) //productId parametre olarak geliyor, imageId ise queryStringden geliyor
    {
        Product product = await _productReadRepository.Table
            .Include(p => p.ProductImageFiles)
            .FirstOrDefaultAsync(p => p.Id == Guid.Parse(productId));

        ProductImageFile productImageFile = product.ProductImageFiles.FirstOrDefault(
            p => p.Id == Guid.Parse(imageId)
        );

        await _productImageFileWriteRepository.RemoveAsync(productImageFile.Id.ToString());
        await _productImageFileWriteRepository.SaveAsync();

        string imageName = productImageFile.Path.Split("/")[1]; // bize / işaretinin gelme ihtimali yok çünkü characteregulatory ile / işaretini boş stringe dönüştürmüştük

        await _storageService.DeleteAsync("product-image", imageName); // fotoğrafı azuredan da sildik

        return Ok();
    }
}

// string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "resource/product-images");: Dosyaların yükleneceği dizinin tam yolu olan uploadPath değişkenini oluşturur. Path.Combine metodu, WebRootPath (wwwroot klasörü) ve "resource/product-images" klasör yolunu birleştirerek dosyaların yükleneceği dizini elde eder.

// foreach (var file in Request.Form.Files): HTTP isteği ile gönderilen dosyaları döngü ile işler. Request.Form.Files, gönderilen dosya verilerine erişimi sağlayan bir koleksiyondur.

// Dosya ile ilgili işlemler yapmak için file değişkenini kullanarak döngü içerisinde devam edilir.

// Dosyanın kaydedileceği tam yol olan fullPath oluşturulur. Path.GetExtension(file.FileName) ile dosya uzantısı alınır ve dosya adının önüne rastgele bir sayı eklenerek dosyanın benzersiz bir isim alması sağlanır.

// using FileStream fileStream = new(...): Dosyanın içeriğini sunucuda belirtilen yola kaydetmek için bir FileStream örneği oluşturulur. using bloğu, fileStream nesnesinin işi bittiğinde otomatik olarak kapatılmasını sağlar.

// Dosyayı sunucuda belirtilen yola FileMode.Create ile oluşturulacak şekilde açar. FileAccess.Write ile dosyaya yazılabilir bir erişim hakkı verilir.

// await file.CopyToAsync(fileStream);: Dosya içeriği, HTTP isteği ile gelen dosya verileriyle fileStream nesnesine asenkron olarak kopyalanır. Dosya yüklemesi için asenkron yöntem kullanılmasının nedeni, sunucunun dosya yüklemelerini eş zamanlı olarak işlemesine olanak sağlamaktır.

// await fileStream.FlushAsync();: Dosyaya yazma işlemi tamamlandığında, fileStream örneği kullanılarak verilerin diske tamamen yazıldığından emin olmak için FlushAsync metodu çağrılır.

// Döngü, diğer dosyaları yüklemek için devam eder.

// return Ok();: Eylem, dosyaların başarıyla yüklendiğini belirten bir Ok cevabı döndürür.
