namespace e_trade_api.application;

public interface IUserService
{
    //new
    Task UpdatePasswordAsync(string userId, string resetToken, string newPassword);
    Task<List<ListUserDTO>> GetAllUsersAsync(int page, int size);
    int TotalUsersCount { get; }
    Task AssignRoleToUserAsnyc(string userId, string[] roles);
    Task<string[]> GetRolesToUserAsync(string userIdOrName);
    Task<bool> HasRolePermissionToEndpointAsync(string name, string code);

    //old

    Task<CreateUserResponseDTO> CreateUser(CreateUserRequestDTO model);
    Task<Token> GoogleLogin(GoogleLoginRequestDTO model);
    Task<LoginUserCommandResponse> LoginUser(LoginUserRequestDTO model);
}
