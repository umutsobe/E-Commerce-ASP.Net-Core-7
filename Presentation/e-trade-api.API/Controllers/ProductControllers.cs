using System.Net;
using e_trade_api.application;
using e_trade_api.domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace e_trade_api.API;

[Route("api/[controller]")]
[ApiController]
public class ProductControllers : ControllerBase
{
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IProductReadRepository _productReadRepository;

    public ProductControllers( //constructer
        IProductReadRepository productReadRepository,
        IProductWriteRepository productWriteRepository
    )
    {
        _productReadRepository = productReadRepository;
        _productWriteRepository = productWriteRepository;
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
}
