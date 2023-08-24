namespace e_trade_api.application;

public interface IAccountService
{
    Task<ListUserDetailsDTO> GetUserDetails(string userId);
    Task<bool> UpdateEmail(string userId, string email);
    Task<bool> UpdateName(string userId, string name);
    Task<List<ListUserOrdersDTO>> ListUserOrders(string userId);
    public Task<Token> UpdateUserPassword(UserPasswordUpdate model);
}
