namespace e_trade_api.application;

public interface IProductRatingService
{
    Task CreateRating(ProductRateRequestDTO model);
    Task<GetUserRatingsResponseDTO> GetUserRatings(GetUserRatingsRequestDTO model);
    Task<GetProductRatingsResponseDTO> GetProductRatings(GetProductRatingsRequestDTO model);
    Task DeleteRating(string ratingId);
    Task UpdateRate(UpdateRateRequestDTO model);
    Task<IsProductReviewPendingResponseDTO> IsProductReviewPending(
        IsProductReviewPendingRequestDTO model
    );
}
