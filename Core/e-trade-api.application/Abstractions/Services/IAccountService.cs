namespace e_trade_api.application;

public interface IAccountService
{
    Task<ListUserDetailsDTO> GetUserDetails();
    Task<bool> UpdateName(string name);
    Task<List<ListUserOrdersDTO>> ListUserOrders();
    Task<Token> UpdateUserPassword(UserPasswordUpdate model);
    Task AddUserAddress(CreateUserAddress model);
    Task<List<GetUserAddress>> GetUserAddresses();
    Task DeleteUserAdsress(string addressId);
    Task<CreateCodeAndSendEmailResponse> UpdateEmailStep1(UpdateUserEmailRequestDTO model);
    Task<CreateCodeAndSendEmailResponse> UpdateEmailStep2(UpdateEmailStep2 model);
}
