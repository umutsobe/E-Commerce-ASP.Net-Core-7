using e_trade_api.application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_trade_api.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Admin")]
public class ApplicationServicesController : ControllerBase
{
    readonly IApplicationService _applicationService;
    readonly IAuthorizationEndpointService _authorizationEndpointService;

    public ApplicationServicesController(
        IApplicationService applicationService,
        IAuthorizationEndpointService authorizationEndpointService
    )
    {
        _applicationService = applicationService;
        _authorizationEndpointService = authorizationEndpointService;
    }

    [HttpGet]
    [AuthorizeDefinition(
        ActionType = ActionType.Reading,
        Definition = "Get Authorize Definition Endpoints",
        Menu = "Application Services"
    )]
    public async Task<IActionResult> GetAuthorizeDefinitionEndpoints()
    {
        List<MenuDTO> datas = _applicationService.GetAuthorizeDefinitionEndpoints(typeof(Program));
        List<NewMenuDTO> newData = new();

        foreach (var menu in datas)
        {
            NewMenuDTO menuDTO = new();
            menuDTO.Actions = new();

            menuDTO.Name = menu.Name;

            foreach (var action in menu.Actions)
            {
                List<string> actionRoles =
                    await _authorizationEndpointService.GetRolesToEndpointAsync(
                        action.Code,
                        action.MenuName
                    );

                NewAction actionDTO =
                    new()
                    {
                        ActionType = action.ActionType,
                        Code = action.Code,
                        Definition = action.Definition,
                        HttpType = action.HttpType,
                        MenuName = action.MenuName,
                        AssignedRoles = actionRoles
                    };

                menuDTO.Actions.Add(actionDTO);
            }
            newData.Add(menuDTO);
        }

        return Ok(newData);
    }
}

public class NewMenuDTO
{
    public string Name { get; set; }
    public List<NewAction> Actions { get; set; }
}

public class NewAction
{
    public string ActionType { get; set; }
    public string HttpType { get; set; }
    public string Definition { get; set; }
    public string Code { get; set; }
    public string MenuName { get; set; }
    public List<string> AssignedRoles { get; set; }
}
