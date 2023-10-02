namespace e_trade_api.application;

public interface IAccountService
{
    Task<ListUserDetailsDTO> GetUserDetails();
    Task<bool> UpdateName(string name);
    Task<List<ListUserOrdersDTO>> ListUserOrders();
    public Task<Token> UpdateUserPassword(UserPasswordUpdate model);
    public Task AddUserAddress(CreateUserAddress model);
    public Task<List<GetUserAddress>> GetUserAddresses();
    public Task DeleteUserAdsress(string addressId);
    Task<CreateCodeAndSendEmailResponse> UpdateEmailStep1(UpdateUserEmailRequestDTO model);
    Task<CreateCodeAndSendEmailResponse> UpdateEmailStep2(UpdateEmailStep2 model);
}
