using e_trade_api.domain;

namespace e_trade_api.application;

public interface IBasketService
{
    public Task<bool> CreateBasket(string userId);
    public Task<Basket> GetBasket(string basketId);
    public Task<string> GetBasketId(string userId);
    public Task<List<BasketItemDTO>> GetBasketItemsAsync(string basketId);
    public Task<ErrorDTO> AddItemToBasketAsync(CreateBasketItemRequestDTO model);
    public Task RemoveBasketItemAsync(string basketItemId);
    public Task<ErrorDTO> UpdateQuantityAsync(UpdateBasketItemRequestDTO basketItem);
}
