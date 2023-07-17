using e_trade_api.application;
using e_trade_api.domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_trade_api.API;

[Route("api/[controller]")]
[ApiController]
public class ProductControllers : ControllerBase
{
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IProductReadRepository _productReadRepository;
    private readonly ICustomerWriteRepository _customerWriteRepository;
    private readonly IOrderWriteRepository _orderWriteRepository;

    public ProductControllers( //constructer
        IProductReadRepository productReadRepository,
        IProductWriteRepository productWriteRepository,
        ICustomerWriteRepository customerWriteRepository,
        IOrderWriteRepository orderWriteRepository
    )
    {
        _productReadRepository = productReadRepository;
        _productWriteRepository = productWriteRepository;
        _customerWriteRepository = customerWriteRepository;
        _orderWriteRepository = orderWriteRepository;
    }

    [HttpGet]
    public async Task Get()
    {
        var id = Guid.Parse("FA7A1A92-1EA7-42F3-9BB7-D5B0E3905187");

        // await _customerWriteRepository.AddAsync(new Customer { Id = id, Name = "Umut" });
        // await _customerWriteRepository.SaveAsync();

        await _orderWriteRepository.AddAsync(
            new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = id,
                Adress = "izmir yamanlar",
                Description = "nbdjkasndxkjsan",
            }
        );
        await _orderWriteRepository.SaveAsync();
    }
}
