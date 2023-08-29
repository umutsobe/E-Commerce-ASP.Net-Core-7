using e_trade_api.application;
using Microsoft.AspNetCore.Mvc;

namespace e_trade_api.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllCategories([FromQuery] int page, int size)
    {
        GetAllCategoriesDTO response = await _categoryService.GetAllCategories(
            new() { Page = page, Size = size }
        );

        return Ok(response);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDTO model)
    {
        await _categoryService.CreateCategory(model.Name);

        return Ok();
    }

    [HttpDelete("[action]/{Id}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] string Id)
    {
        await _categoryService.DeleteCategory(Id);

        return Ok();
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateCategoryName([FromBody] UpdateCategoryNameDTO model)
    {
        await _categoryService.UpdateCategoryName(model);

        return Ok();
    }
}
