using e_trade_api.application;
using e_trade_api.domain;
using e_trade_api.domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.Persistence;

public class AccountService : IAccountService
{
    readonly UserManager<AppUser> _userManager;
    readonly SignInManager<AppUser> _signInManager;
    readonly ITokenHandler _tokenHandler;

    readonly IOrderReadRepository _orderReadRepository;

    public AccountService(
        UserManager<AppUser> userManager,
        IOrderReadRepository orderReadRepository,
        SignInManager<AppUser> signInManager,
        ITokenHandler tokenHandler
    )
    {
        _userManager = userManager;
        _orderReadRepository = orderReadRepository;
        _signInManager = signInManager;
        _tokenHandler = tokenHandler;
    }

    public async Task<ListUserDetailsDTO> GetUserDetails(string userId)
    {
        AppUser appUser = await _userManager.FindByIdAsync(userId);

        if (appUser != null)
        {
            return new() { Name = appUser.Name, Email = appUser.Email, };
        }

        throw new Exception("Kullanıcı Bulunamadı");
    }

    public async Task<bool> UpdateEmail(string userId, string email)
    {
        AppUser appUser = await _userManager.FindByIdAsync(userId);
        IdentityResult result = await _userManager.SetEmailAsync(appUser, email);

        return result.Succeeded;
    }

    public async Task<bool> UpdateName(string userId, string name)
    {
        AppUser appuser = await _userManager.FindByIdAsync(userId);

        appuser.Name = name;
        IdentityResult result = await _userManager.UpdateAsync(appuser);

        return result.Succeeded;
    }

    public async Task<List<ListUserOrdersDTO>> ListUserOrders(string userId)
    {
        List<Order> orders = await _orderReadRepository.Table
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedDate)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync();

        List<ListUserOrdersDTO> orderDTOs = new List<ListUserOrdersDTO>();

        if (orders != null)
        {
            foreach (var order in orders)
            {
                ListUserOrdersDTO orderDTO =
                    new()
                    {
                        Adress = order.Adress,
                        CreatedDate = order.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        OrderCode = order.OrderCode,
                        OrderItems = new List<ListUserOrderItemsDTO>(),
                        TotalPrice = order.OrderItems.Sum(o => o.Price * o.Quantity)
                    };

                foreach (OrderItem orderItem in order.OrderItems)
                {
                    ListUserOrderItemsDTO orderItemDTO =
                        new()
                        {
                            Name = orderItem.Product.Name,
                            Price = orderItem.Price,
                            Quantity = orderItem.Quantity,
                            ProductId = orderItem.ProductId.ToString(),
                        };

                    orderDTO.OrderItems.Add(orderItemDTO);
                }

                orderDTOs.Add(orderDTO);
            }

            return orderDTOs;
        }
        return new();
    }

    public async Task<Token> UpdateUserPassword(UserPasswordUpdate model)
    {
        AppUser appUser = await _userManager.FindByIdAsync(model.UserId);

        if (appUser != null)
        {
            IdentityResult result = await _userManager.ChangePasswordAsync(
                appUser,
                model.OldPassword,
                model.NewPassword
            );

            if (result.Succeeded)
            { //kullanıcının giriş yapması için token üretme
                await _userManager.UpdateSecurityStampAsync(appUser);
                SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(
                    appUser,
                    model.NewPassword,
                    false
                );

                if (signInResult.Succeeded)
                {
                    return await _tokenHandler.CreateAccessToken(180, appUser.Id);
                }
            }
        }

        throw new Exception("Bir sorunla karşılaşıldı.");
    }
}
