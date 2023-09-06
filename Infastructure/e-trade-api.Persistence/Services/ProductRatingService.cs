using e_trade_api.application;
using e_trade_api.domain;
using e_trade_api.domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.Persistence;

public class ProductRatingService : IProductRatingService
{
    readonly IProductRatingReadRepository _productRatingReadRepository;
    readonly IProductRatingWriteRepository _productRatingWriteRepository;
    readonly IProductReadRepository _productReadRepository;
    readonly UserManager<AppUser> _userManager;
    readonly IOrderItemReadRepository _orderItemReadRepository;

    public ProductRatingService(
        IProductRatingWriteRepository productRatingWriteRepository,
        IProductRatingReadRepository productRatingReadRepository,
        IProductReadRepository productReadRepository,
        UserManager<AppUser> userManager,
        IOrderItemReadRepository orderItemReadRepository
    )
    {
        _productRatingWriteRepository = productRatingWriteRepository;
        _productRatingReadRepository = productRatingReadRepository;
        _productReadRepository = productReadRepository;
        _userManager = userManager;
        _orderItemReadRepository = orderItemReadRepository;
    }

    public async Task DeleteRating(string ratingId)
    {
        await _productRatingWriteRepository.RemoveAsync(ratingId);
        await _productRatingWriteRepository.SaveAsync();
    }

    public async Task<GetProductRatingsResponseDTO> GetProductRatings(
        GetProductRatingsRequestDTO model
    )
    {
        Product? product = await _productReadRepository.GetByIdAsync(model.ProductId);

        if (product != null)
        {
            var query = _productRatingReadRepository.Table
                .Where(pr => pr.ProductId == Guid.Parse(model.ProductId))
                .Include(pr => pr.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(model.Keyword))
                query = query.Where(p => EF.Functions.Like(p.Comment, $"%{model.Keyword}%"));

            int totalProductRatingCount = await query.CountAsync();

            if (model.SortType != null)
            {
                if (model.SortType == "newDate")
                    query = query.OrderByDescending(pr => pr.CreatedDate);
                else if (model.SortType == "oldDate")
                    query = query.OrderBy(pr => pr.CreatedDate);
                else if (model.SortType == "best")
                    query = query.OrderByDescending(pr => pr.Star);
                else if (model.SortType == "worst")
                    query = query.OrderBy(pr => pr.Star);
            }
            else
            {
                query = query.OrderByDescending(pr => pr.CreatedDate);
            }

            query = query.Skip(model.Page * model.Size).Take(model.Size);

            var filteredRatings = await query
                .Select(
                    pr =>
                        new
                        {
                            pr.Star,
                            pr.Comment,
                            pr.User.Name,
                            pr.CreatedDate
                        }
                )
                .ToListAsync();

            GetProductRatingsResponseDTO productRatingsDTO = new();
            productRatingsDTO.Ratings = new();

            productRatingsDTO.TotalProductRatingCount = totalProductRatingCount;
            foreach (var productRating in filteredRatings)
            {
                GetProductRatingResponseDTO productRatingDTO =
                    new()
                    {
                        Comment = productRating.Comment,
                        CreatedDate = productRating.CreatedDate,
                        Star = productRating.Star,
                        UserName = NameSurnameHide.Hide(productRating.Name),
                    };

                productRatingsDTO.Ratings.Add(productRatingDTO);
            }
            return productRatingsDTO;
        }

        throw new Exception("Product Not Found");
    }

    public async Task<GetUserRatingsResponseDTO> GetUserRatings(GetUserRatingsRequestDTO model)
    {
        AppUser user = await _userManager.FindByIdAsync(model.UserId);

        if (user != null)
        {
            var query = _productRatingReadRepository.Table
                .Where(pr => pr.UserId == model.UserId)
                .Include(pr => pr.Product)
                .AsQueryable();

            int totalUserRatingCount = await query.CountAsync();

            if (model.SortType != null)
            {
                if (model.SortType == "newDate")
                    query = query.OrderBy(pr => pr.CreatedDate);
                else if (model.SortType == "oldDate")
                    query = query.OrderByDescending(pr => pr.CreatedDate);
            }

            query = query.Skip(model.Page * model.Size).Take(model.Size);

            var filteredRatings = await query
                .Select(
                    pr =>
                        new
                        {
                            pr.Star,
                            pr.Comment,
                            pr.CreatedDate,
                            pr.Product.Url
                        }
                )
                .ToListAsync();

            GetUserRatingsResponseDTO userRatingsDTO = new();
            userRatingsDTO.Ratings = new();

            userRatingsDTO.TotaluserRatingCount = totalUserRatingCount;

            foreach (var productRating in filteredRatings)
            {
                GetUserRatingResponseDTO userRatingDTO =
                    new()
                    {
                        Comment = productRating.Comment,
                        CreatedDate = productRating.CreatedDate,
                        Star = productRating.Star,
                        ProductUrlId = productRating.Url
                    };

                userRatingsDTO.Ratings.Add(userRatingDTO);
            }
            return userRatingsDTO;
        }

        throw new Exception("User Not Found");
    }

    public async Task CreateRating(ProductRateRequestDTO model)
    {
        IsProductReviewPendingResponseDTO ratingStatus = await IsProductReviewPending(
            new() { ProductId = model.ProductId, UserId = model.UserId }
        );

        if (ratingStatus.State == IsProductReviewPendingStatus.BuyedAndNotRating.ToString())
        {
            Product product = await _productReadRepository.GetByIdAsync(model.ProductId);
            AppUser user = await _userManager.FindByIdAsync(model.UserId);

            if (product != null && user != null)
            {
                await _productRatingWriteRepository.AddAsync(
                    new()
                    {
                        Id = Guid.NewGuid(),
                        ProductId = Guid.Parse(model.ProductId),
                        Comment = model.Comment,
                        Star = model.Star,
                        UserId = model.UserId,
                    }
                );
                await _productRatingWriteRepository.SaveAsync();
            }
        }
        else if (ratingStatus.State == IsProductReviewPendingStatus.NotBuyed.ToString())
            throw new Exception("Satın almadığınız ürüne yorum yapamazsınız");
        else if (ratingStatus.State == IsProductReviewPendingStatus.BuyedAndHasRating.ToString())
            throw new Exception("Bir ürüne 1 kez yorum yapabilirsiniz");
    }

    public async Task UpdateRate(UpdateRateRequestDTO model)
    {
        ProductRating productRating = await _productRatingReadRepository.GetByIdAsync(model.RateId);

        if (productRating != null)
        {
            productRating.Star = model.Star;
            productRating.Comment = model.Comment;

            await _productRatingWriteRepository.SaveAsync();
        }
    }

    public async Task<IsProductReviewPendingResponseDTO> IsProductReviewPending(
        IsProductReviewPendingRequestDTO model
    ) //kullancı ürünü almamışsa false, almışsa yorum yapmışsa false, almış yorum yapmamışsa true
    {
        OrderItem? orderItems = await _orderItemReadRepository.Table
            .Include(oi => oi.Order)
            .Where(
                oi => oi.ProductId == Guid.Parse(model.ProductId) && oi.Order.UserId == model.UserId
            )
            .FirstOrDefaultAsync();

        if (orderItems != null)
        { //kullanıcı ürünü almış
            ProductRating? productRating = await _productRatingReadRepository.Table
                .Where(
                    pr => pr.ProductId == Guid.Parse(model.ProductId) && pr.UserId == model.UserId
                )
                .FirstOrDefaultAsync();

            if (productRating != null) //kullanıcı ürüne yorum yapmış
                return new() { State = IsProductReviewPendingStatus.BuyedAndHasRating.ToString() };

            if (productRating == null) // kullanıcı ürüne yorum yapmamış
                return new() { State = IsProductReviewPendingStatus.BuyedAndNotRating.ToString() };
        }

        return new() { State = IsProductReviewPendingStatus.NotBuyed.ToString() }; //kullanıcı ürünü almamış
    }
}
