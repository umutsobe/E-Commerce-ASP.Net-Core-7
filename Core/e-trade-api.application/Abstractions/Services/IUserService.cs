namespace e_trade_api.application;

public interface IUserService
{
    Task UpdatePasswordAsync(string userId, string resetToken, string newPassword);
    Task<List<ListUser>> GetAllUsersAsync(int page, int size);
    int TotalUsersCount { get; }
    Task AssignRoleToUserAsnyc(string userId, string[] roles);
    Task<string[]> GetRolesToUserAsync(string userIdOrName);
    Task<bool> HasRolePermissionToEndpointAsync(string name, string code);
}
