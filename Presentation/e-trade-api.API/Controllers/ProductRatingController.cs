using e_trade_api.application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_trade_api.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductRatingController : ControllerBase
{
    readonly IProductRatingService _productRatingService;

    public ProductRatingController(IProductRatingService productRatingService)
    {
        _productRatingService = productRatingService;
    }

    [HttpDelete("[action]/{ratingId}")]
    public async Task<ActionResult> DeleteRating([FromRoute] string ratingId)
    {
        await _productRatingService.DeleteRating(ratingId);
        return Ok();
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> CreateRating([FromBody] ProductRateRequestDTO model)
    {
        await _productRatingService.CreateRating(model);
        return Ok();
    }

    [HttpGet("[action]")]
    public async Task<ActionResult> GetProductRatings([FromQuery] GetProductRatingsRequestDTO model)
    {
        GetProductRatingsResponseDTO response = await _productRatingService.GetProductRatings(
            model
        );
        return Ok(response);
    }

    [HttpGet("[action]")]
    public async Task<ActionResult> GetUserRatings([FromQuery] GetUserRatingsRequestDTO model)
    {
        GetUserRatingsResponseDTO response = await _productRatingService.GetUserRatings(model);
        return Ok(response);
    }

    [HttpPut("[action]")]
    public async Task<ActionResult> UpdateRate([FromBody] UpdateRateRequestDTO model)
    {
        await _productRatingService.UpdateRate(model);
        return Ok();
    }

    [HttpGet("[action]")]
    public async Task<ActionResult> IsProductReviewPending(
        [FromQuery] IsProductReviewPendingRequestDTO model
    )
    {
        IsProductReviewPendingResponseDTO response =
            await _productRatingService.IsProductReviewPending(model);
        return Ok(response);
    }
}
