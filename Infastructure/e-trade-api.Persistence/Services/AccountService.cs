using e_trade_api.application;
using e_trade_api.domain;
using e_trade_api.domain.Entities;
using e_trade_api.Persistence.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.Persistence;

public class AccountService : IAccountService
{
    readonly UserManager<AppUser> _userManager;
    readonly SignInManager<AppUser> _signInManager;
    readonly ITokenHandler _tokenHandler;
    readonly IAddressWriteRepository _addressWriteRepository;
    readonly IAddressReadRepository _addressReadRepository;
    readonly IOrderReadRepository _orderReadRepository;
    readonly IMailService _mailService;
    readonly ITwoFactorAuthenticationWriteRepository _twoFactorAuthenticationWriteRepository;
    readonly ITwoFactorAuthenticationReadRepository _twoFactorAuthenticationReadRepository;
    readonly ITwoFactorAuthenticationService _twoFactorAuthenticationService;

    public AccountService(
        UserManager<AppUser> userManager,
        IOrderReadRepository orderReadRepository,
        SignInManager<AppUser> signInManager,
        ITokenHandler tokenHandler,
        IAddressWriteRepository addressWriteRepository,
        IAddressReadRepository addressReadRepository,
        IMailService mailService,
        ITwoFactorAuthenticationWriteRepository twoFactorAuthenticationWriteRepository,
        ITwoFactorAuthenticationReadRepository twoFactorAuthenticationReadRepository,
        ITwoFactorAuthenticationService twoFactorAuthenticationService
    )
    {
        _userManager = userManager;
        _orderReadRepository = orderReadRepository;
        _signInManager = signInManager;
        _tokenHandler = tokenHandler;
        _addressWriteRepository = addressWriteRepository;
        _addressReadRepository = addressReadRepository;
        _mailService = mailService;
        _twoFactorAuthenticationWriteRepository = twoFactorAuthenticationWriteRepository;
        _twoFactorAuthenticationReadRepository = twoFactorAuthenticationReadRepository;
        _twoFactorAuthenticationService = twoFactorAuthenticationService;
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

    public async Task AddUserAddress(CreateUserAddress model)
    {
        AppUser appUser = await _userManager.FindByIdAsync(model.UserId);

        if (appUser != null)
        {
            Adress adress =
                new()
                {
                    Id = Guid.NewGuid(),
                    UserId = appUser.Id,
                    Definition = model.Definition,
                    FullAdress = model.Address,
                };

            await _addressWriteRepository.AddAsync(adress);

            await _addressWriteRepository.SaveAsync();
        }
    }

    public async Task<List<GetUserAddress>> GetUserAddresses(string userId)
    {
        List<Adress> databaseAdresses = await _addressReadRepository.Table
            .Where(a => a.UserId == userId)
            .ToListAsync();

        List<GetUserAddress> addressesDto = new();

        foreach (var databaseAddress in databaseAdresses)
        {
            GetUserAddress adressDto =
                new()
                {
                    Id = databaseAddress.Id.ToString(),
                    Address = databaseAddress.FullAdress,
                    Definition = databaseAddress.Definition
                };

            addressesDto.Add(adressDto);
        }

        return addressesDto;
    }

    public async Task DeleteUserAdsress(string addressId)
    {
        Adress adress = await _addressReadRepository.GetByIdAsync(addressId);

        if (adress != null)
        {
            await _addressWriteRepository.RemoveAsync(adress.Id.ToString());

            await _addressWriteRepository.SaveAsync();
        }
    }

    public async Task<CreateCodeAndSendEmailResponse> UpdateEmailStep1(
        UpdateUserEmailRequestDTO model
    )
    {
        AppUser? appUser = await _userManager.FindByEmailAsync(model.NewEmail);
        if (appUser != null)
            return new()
            {
                Message = "This email address is already in use. Please try another email address.",
                Succeeded = false
            };

        appUser = await _userManager.FindByIdAsync(model.UserId);
        if (appUser == null)
            return new() { Message = "Error", Succeeded = false };

        bool isPasswordValid = await _userManager.CheckPasswordAsync(appUser, model.Password);

        if (isPasswordValid == false)
            return new() { Message = "Wrong Password", Succeeded = false };

        if (!await _twoFactorAuthenticationService.CheckCodeAttempts(model.UserId))
            return new()
            {
                Message = "Çok fazla denemede bulundunuz. 24 saat sonra tekrar deneyin.",
                Succeeded = false
            };

        //her şey doğru

        string code = CreateVerificationCode.CreateCode();

        await _twoFactorAuthenticationWriteRepository.AddAsync(
            new()
            {
                Id = Guid.NewGuid(),
                Code = code,
                ExpirationDate = DateTime.UtcNow.AddMinutes(3),
                IsUsed = false,
                UserId = appUser.Id,
                User = appUser
            }
        );
        await _twoFactorAuthenticationWriteRepository.SaveAsync();

        appUser.NewEmailControl = model.NewEmail;
        await _userManager.UpdateAsync(appUser);

        await _mailService.SendEmailVerificationCode(model.NewEmail, code);

        return new()
        {
            Message =
                "Yeni e-posta adresinize gönderilen doğrulama kodunu kullanarak işlemi tamamlayabilirsiniz.",
            Succeeded = true
        };
    }

    public async Task<CreateCodeAndSendEmailResponse> UpdateEmailStep2(UpdateEmailStep2 model)
    {
        AppUser? appUser = await _userManager.FindByIdAsync(model.UserId);

        if (appUser == null)
            return new() { Message = "Kullanıcı bulunamadı", Succeeded = false };

        if (!await _twoFactorAuthenticationService.CheckCodeAttempts(model.UserId))
            return new()
            {
                Message = "Çok fazla denemede bulundunuz. 24 saat sonra tekrar deneyin.",
                Succeeded = false
            };

        TwoFactorAuthentication? twoFactor = await _twoFactorAuthenticationReadRepository.Table
            .Where(
                t =>
                    t.UserId == model.UserId
                    && t.Code == model.Code
                    && t.IsUsed == false
                    && t.ExpirationDate > DateTime.UtcNow //exp saat 12.05, datenow 12.04 ise true
            )
            .FirstOrDefaultAsync(); //bulunması doğru

        if (twoFactor == null)
            return new() { Message = "Girdiğiniz kod hatalı. Tekrar deneyiniz", Succeeded = false };

        //valid

        await _userManager.SetEmailAsync(appUser, appUser.NewEmailControl);
        appUser.EmailConfirmed = true;

        //kodları kaldırma
        List<TwoFactorAuthentication> twoFactorAuthentications =
            await _twoFactorAuthenticationReadRepository.Table
                .Where(t => t.UserId == model.UserId)
                .ToListAsync();

        _twoFactorAuthenticationWriteRepository.RemoveRange(twoFactorAuthentications);

        await _twoFactorAuthenticationWriteRepository.SaveAsync();
        await _userManager.UpdateAsync(appUser);

        return new() { Message = "Yeni mailiniz başarıyla onaylandı.", Succeeded = true };
    }
}
