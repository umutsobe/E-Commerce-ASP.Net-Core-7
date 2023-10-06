namespace e_trade_api.application;

public interface IRoleService
{
    Task<(object, int)> GetAllRoles(int page, int size);
    Task<(string id, string name)> GetRoleById(string id);
    Task<bool> CreateRole(string name);
    Task<bool> DeleteRole(string id);
    Task<bool> UpdateRole(string id, string name);
}
