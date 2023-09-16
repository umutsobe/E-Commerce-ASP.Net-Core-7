using System.Net.Http;
using e_trade_api.application;
using e_trade_api.domain;
using e_trade_api.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.Persistence;

public class BasketService : IBasketService
{
    readonly IBasketWriteRepository _basketWriteRepository;
    readonly IBasketReadRepository _basketReadRepository;
    readonly IBasketItemWriteRepository _basketItemWriteRepository;
    readonly IBasketItemReadRepository _basketItemReadRepository;
    readonly IProductReadRepository _productReadRepository;

    public BasketService(
        IBasketWriteRepository basketWriteRepository,
        IBasketItemWriteRepository basketItemWriteRepository,
        IBasketItemReadRepository basketItemReadRepository,
        IBasketReadRepository basketReadRepository,
        IProductReadRepository productReadRepository
    )
    {
        _basketWriteRepository = basketWriteRepository;
        _basketItemWriteRepository = basketItemWriteRepository;
        _basketItemReadRepository = basketItemReadRepository;
        _basketReadRepository = basketReadRepository;
        _productReadRepository = productReadRepository;
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

    public async Task<ErrorDTO> AddItemToBasketAsync(CreateBasketItemRequestDTO model)
    {
        Basket basket = await GetBasket(model.BasketId);

        BasketItem? _basketItem = await _basketItemReadRepository.GetSingleAsync( //burada basketItem daha önce var mı onu kontrol ediyoruz
            bi => bi.BasketId == basket.Id && bi.ProductId == Guid.Parse(model.ProductId)
        );
        Product? product = await _productReadRepository.Table.FirstOrDefaultAsync(
            p => p.Id == Guid.Parse(model.ProductId)
        );

        if (product == null)
            return new() { Succeeded = false, Message = "Product not found" };

        if (
            (_basketItem == null ? model.Quantity : _basketItem.Quantity + model.Quantity)
            <= product.Stock
        //basketitem daha önce eklenmemişse direkt requestten gelen item quantity sayısını kullan. daha önce eklenmişse de veritabanı + gelen istek sayısı toplayıp ona göre kontrol et
        )
        {
            if (_basketItem != null)
            {
                _basketItem.Quantity += model.Quantity;
                product.TotalBasketAdded += model.Quantity;
            }
            else // basketItem daha önce eklenmemişse
            {
                await _basketItemWriteRepository.AddAsync( //yeni basketItem oluştur
                    new()
                    {
                        BasketId = basket.Id,
                        ProductId = Guid.Parse(model.ProductId),
                        Quantity = model.Quantity,
                    }
                );
                product.TotalBasketAdded += model.Quantity;
            }

            await _basketItemWriteRepository.SaveAsync();
            return new() { Succeeded = true };
        }
        else //quantity bunu karşılamıyorsa
        {
            return new()
            {
                Succeeded = false,
                Message =
                    "The product's stock quantity doesn't cover this. Please try again later. Check your cart."
            };
        }
    }

    public async Task<List<BasketItemDTO>> GetBasketItemsAsync(string basketId)
    {
        Basket? basket = await GetBasket(basketId);

        Basket? result = await _basketReadRepository.Table
            .Include(b => b.BasketItems)
            .ThenInclude(bi => bi.Product)
            .FirstOrDefaultAsync(b => b.Id == basket.Id);

        if (result == null)
            throw new Exception("Sepet bulunamadı");

        List<BasketItemDTO> basketItemDTOs = new();

        foreach (var basketItem in result.BasketItems)
        {
            BasketItemDTO basketItemDTO = new();

            if (basketItem.Quantity <= basketItem.Product.Stock)
            {
                basketItemDTO = new()
                {
                    BasketItemId = basketItem.Id.ToString(),
                    ProductId = basketItem.ProductId.ToString(),
                    Quantity = basketItem.Quantity,
                    Name = basketItem.Product.Name,
                    Price = basketItem.Product.Price,
                    ProductStock = basketItem.Product.Stock,
                };
                basketItemDTOs.Add(basketItemDTO);
            }
            else //sepetteki ürün adedi product stoğundan fazla
            {
                if (basketItem.Product.Stock > 0) //product stoğu 0dan büyük ise basket adedi product adedine eşit olsun
                {
                    basketItem.Quantity = basketItem.Product.Stock;
                    await _basketItemWriteRepository.SaveAsync();
                }
                else //product stoğu 0 ise de o basketItem'ı kaldır
                {
                    await _basketItemWriteRepository.RemoveAsync(basketItem.Id.ToString());
                    await _basketItemWriteRepository.SaveAsync();
                }
            }
        }

        return basketItemDTOs;
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

    public async Task<ErrorDTO> UpdateQuantityAsync(UpdateBasketItemRequestDTO basketItemDTO)
    {
        BasketItem? databaseBasketItem = await _basketItemReadRepository.GetByIdAsync(
            basketItemDTO.BasketItemId
        );

        Product? product = await _productReadRepository.Table.FirstOrDefaultAsync(
            p => p.Id == databaseBasketItem.ProductId
        );

        if (product == null || databaseBasketItem == null)
            return new() { Succeeded = false, Message = "Product not found" };

        if (databaseBasketItem != null)
        {
            if (basketItemDTO.Quantity <= product.Stock)
            {
                if (databaseBasketItem.Quantity <= basketItemDTO.Quantity) //sadece quantity artarken arttır. quantity azalırken arttırma
                {
                    product.TotalBasketAdded++;
                    await _basketItemWriteRepository.SaveAsync();
                }

                databaseBasketItem.Quantity = basketItemDTO.Quantity;
                await _basketItemWriteRepository.SaveAsync();
                return new() { Succeeded = true };
            }
        }

        return new()
        {
            Succeeded = false,
            Message = "The product's stock quantity doesn't cover this. Please try again later."
        };
    }
}
