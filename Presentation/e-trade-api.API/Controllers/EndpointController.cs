using e_trade_api.application;
using e_trade_api.domain;
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

    public EndpointController(
        IApplicationService applicationService,
        IMenuReadRepository menuReadRepository,
        IMenuWriteRepository menuWriteRepository,
        IEndpointReadRepository endpointReadRepository,
        IEndpointWriteRepository endpointWriteRepository
    )
    {
        _applicationService = applicationService;
        _menuReadRepository = menuReadRepository;
        _menuWriteRepository = menuWriteRepository;
        _endpointReadRepository = endpointReadRepository;
        _endpointWriteRepository = endpointWriteRepository;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> SetMenu()
    {
        List<Menu> _menus = await _menuReadRepository.Table.ToListAsync(); //tüm menuleri siliyoruz

        if (_menus != null)
        {
            _menuWriteRepository.RemoveRange(_menus);
            await _menuWriteRepository.SaveAsync(); //sildik
        }

        List<MenuDTO> menus = _applicationService.GetAuthorizeDefinitionEndpoints(typeof(Program));

        foreach (MenuDTO menu in menus)
        {
            Menu _menu = new() { Name = menu.Name, Id = Guid.NewGuid() };
            await _menuWriteRepository.AddAsync(_menu);
        }
        await _menuWriteRepository.SaveAsync();

        return Ok();
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> SetEndpoint()
    {
        List<Endpoint> endpoints = await _endpointReadRepository.Table.ToListAsync(); //tüm endpointleri siliyoruz

        if (endpoints != null)
        {
            _endpointWriteRepository.RemoveRange(endpoints);

            await _endpointWriteRepository.SaveAsync(); //sildik
        }

        List<MenuDTO> menus = _applicationService.GetAuthorizeDefinitionEndpoints(typeof(Program));

        foreach (MenuDTO menu in menus)
        {
            foreach (Action action in menu.Actions)
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
        await _endpointWriteRepository.SaveAsync();
        return Ok();
    }
}
