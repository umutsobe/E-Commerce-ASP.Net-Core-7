using System.Text;
using e_trade_api.application;
using e_trade_api.domain;
using e_trade_api.domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Action = e_trade_api.application.Action;
using Endpoint = e_trade_api.domain.Endpoint;

namespace e_trade_api.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EndpointController : ControllerBase
{
    readonly IApplicationService _applicationService;
    readonly IMenuReadRepository _menuReadRepository;
    readonly IMenuWriteRepository _menuWriteRepository;
    readonly IEndpointReadRepository _endpointReadRepository;
    readonly IEndpointWriteRepository _endpointWriteRepository;
    readonly IProductReadRepository _productReadRepository;
    readonly IHttpContextAccessor _httpContextAccessor;
    readonly IConfiguration _configuration;

    public EndpointController(
        IApplicationService applicationService,
        IMenuReadRepository menuReadRepository,
        IMenuWriteRepository menuWriteRepository,
        IEndpointReadRepository endpointReadRepository,
        IEndpointWriteRepository endpointWriteRepository,
        IProductReadRepository productReadRepository,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration
    )
    {
        _applicationService = applicationService;
        _menuReadRepository = menuReadRepository;
        _menuWriteRepository = menuWriteRepository;
        _endpointReadRepository = endpointReadRepository;
        _endpointWriteRepository = endpointWriteRepository;
        _productReadRepository = productReadRepository;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = "Auth")]
    [AuthorizeDefinition(
        Menu = "Endpoint",
        ActionType = ActionType.Updating,
        Definition = "Update All Menus And Endpoints"
    )]
    public async Task<ActionResult> UpdateMenusAndEndpoints()
    {
        ////////////// menu set

        List<Menu> databaseMenus = await _menuReadRepository.Table.ToListAsync(); //database'teki menüleri aldık

        List<MenuDTO> menus = _applicationService.GetAuthorizeDefinitionEndpoints(typeof(Program)); //programdaki menüleri aldık

        foreach (MenuDTO menu in menus)
        {
            if (databaseMenus.FirstOrDefault(m => m.Name == menu.Name) == null) //eğer  programdan aldığımız menü adında database'de bir menü bulunmuyorsa o menüyü ekle
            {
                Menu _menu = new() { Name = menu.Name, Id = Guid.NewGuid() };
                await _menuWriteRepository.AddAsync(_menu);
            }
        }
        

        await _menuWriteRepository.SaveAsync();

        ////////////// endpoint set

        List<Endpoint> databaseEndpoints = await _endpointReadRepository.Table.ToListAsync(); //database'teki endpointleri aldık

        foreach (MenuDTO menu in menus)
        {
            foreach (Action action in menu.Actions)
            {
                if (databaseEndpoints.FirstOrDefault(e => e.Code == action.Code) == null) //eğer bu endpoint koduna ait bir kod bulunmuyorsa yeni bir endpoint oluşturuyoruz
                {
                    Menu? _menu = await _menuReadRepository.Table
                        .Where(m => m.Name == action.MenuName)
                        .FirstOrDefaultAsync();

                    if (_menu != null)
                    {
                        Endpoint endpoint =
                            new()
                            {
                                ActionType = action.ActionType,
                                Code = action.Code,
                                Definition = action.Definition,
                                Id = Guid.NewGuid(),
                                HttpType = action.HttpType,
                                MenuId = _menu.Id
                            };

                        await _endpointWriteRepository.AddAsync(endpoint);
                    }
                }
            }
        }
        await _endpointWriteRepository.SaveAsync();

        return Ok();
    }

    [HttpGet("[action]")]
    //for prerender
    public async Task<IActionResult> GetProductRoutes()
    {
        List<Product> products = await _productReadRepository.GetAll().ToListAsync();
        StringBuilder urls = new();

        foreach (var product in products)
        {
            string url = "/product/" + product.Url;
            urls.AppendLine(url);
        }
        return Ok(urls.ToString());
    }

    [HttpGet("[action]")]
    //for prerender
    public async Task<IActionResult> GetUsersIP()
    {
        var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        return Ok(ip);
    }
}
