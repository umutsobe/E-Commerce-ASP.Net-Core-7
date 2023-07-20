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

    public ProductControllers( //constructer
        IProductReadRepository productReadRepository,
        IProductWriteRepository productWriteRepository
    )
    {
        _productReadRepository = productReadRepository;
        _productWriteRepository = productWriteRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok("merhaba");
    }
}
