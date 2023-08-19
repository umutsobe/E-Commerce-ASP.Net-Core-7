namespace e_trade_api.application;

public interface IApplicationService
{
    List<MenuDTO> GetAuthorizeDefinitionEndpoints(Type type);
}
