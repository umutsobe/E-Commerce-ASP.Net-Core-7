using e_trade_api.application;
using e_trade_api.domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.Persistence;

public class BasketService : IBasketService
{
    readonly IHttpContextAccessor _httpContextAccessor;
    readonly UserManager<AppUser> _userManager;
    readonly IOrderReadRepository _orderReadRepository;
    readonly IBasketWriteRepository _basketWriteRepository;
    readonly IBasketReadRepository _basketReadRepository;
    readonly IBasketItemWriteRepository _basketItemWriteRepository;
    readonly IBasketItemReadRepository _basketItemReadRepository;

    public BasketService(
        IHttpContextAccessor httpContextAccessor,
        UserManager<AppUser> userManager,
        IOrderReadRepository orderReadRepository,
        IBasketWriteRepository basketWriteRepository,
        IBasketItemWriteRepository basketItemWriteRepository,
        IBasketItemReadRepository basketItemReadRepository,
        IBasketReadRepository basketReadRepository
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _orderReadRepository = orderReadRepository;
        _basketWriteRepository = basketWriteRepository;
        _basketItemWriteRepository = basketItemWriteRepository;
        _basketItemReadRepository = basketItemReadRepository;
        _basketReadRepository = basketReadRepository;
    }

    public async Task<bool> CreateBasket(string userId)
    {
        await _basketWriteRepository.AddAsync(
            new Basket() { Id = Guid.NewGuid(), UserId = userId }
        );
        await _basketWriteRepository.SaveAsync();
        return true;
    }

    public async Task<Basket> GetBasket(string basketId)
    {
        return await _basketReadRepository.GetByIdAsync(basketId);
    }

    public async Task<string> GetBasketId(string userId)
    {
        Basket? basket = await _basketReadRepository.Table.FirstOrDefaultAsync(
            b => b.UserId == userId
        );

        return basket.Id.ToString();
    }

    public async Task AddItemToBasketAsync(VM_Create_BasketItem basketItem, string basketId)
    {
        Basket basket = await GetBasket(basketId);
        BasketItem _basketItem = await _basketItemReadRepository.GetSingleAsync( //burada basketItem daha önce var mı onu kontrol ediyoruz
            bi => bi.BasketId == basket.Id && bi.ProductId == Guid.Parse(basketItem.ProductId)
        );

        if (_basketItem != null) //basketItem daha önce eklenmişse
            _basketItem.Quantity++; // miktarı arttır
        else // basketItem daha önce eklenmemişse
            await _basketItemWriteRepository.AddAsync( //yeni basketItem oluştur
                new()
                {
                    BasketId = basket.Id,
                    ProductId = Guid.Parse(basketItem.ProductId),
                    Quantity = basketItem.Quantity,
                }
            );

        await _basketItemWriteRepository.SaveAsync();
    }

    public async Task<List<BasketItem>> GetBasketItemsAsync(string basketId)
    {
        Basket? basket = await GetBasket(basketId);

        Basket? result = await _basketReadRepository.Table
            .Include(b => b.BasketItems)
            .ThenInclude(bi => bi.Product)
            .FirstOrDefaultAsync(b => b.Id == basket.Id);

        return result.BasketItems.ToList();
    }

    public async Task RemoveBasketItemAsync(string basketItemId)
    {
        BasketItem? basketItem = await _basketItemReadRepository.GetByIdAsync(basketItemId);
        if (basketItem != null)
        {
            _basketItemWriteRepository.Remove(basketItem);
            await _basketItemWriteRepository.SaveAsync();
        }
    }

    public async Task UpdateQuantityAsync(VM_Update_BasketItem basketItem)
    {
        BasketItem? _basketItem = await _basketItemReadRepository.GetByIdAsync(
            basketItem.BasketItemId
        );
        if (_basketItem != null)
        {
            _basketItem.Quantity = basketItem.Quantity;
            await _basketItemWriteRepository.SaveAsync();
        }
    }
}
