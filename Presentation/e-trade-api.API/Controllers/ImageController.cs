using e_trade_api.application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_trade_api.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    private readonly IImageFileService _imageFileService;

    public ImageController(IImageFileService imageFileService)
    {
        _imageFileService = imageFileService;
    }

    [HttpPost("[action]")]
    [Authorize(AuthenticationSchemes = "Auth")]
    [AuthorizeDefinition(
        Menu = "ImageFile",
        ActionType = ActionType.Writing,
        Definition = "Upload Image"
    )]
    public async Task<IActionResult> UploadImage([FromQuery] UploadImageRequestDTO model)
    {
        model.Files = Request.Form.Files; //dosyaları bodyi querystring veya diğer şekilde yakalayamayız
        await _imageFileService.UploadImage(model);

        return Ok();
    }

    [HttpDelete("[action]/{imageId}")]
    [AuthorizeDefinition(
        Menu = "ImageFile",
        ActionType = ActionType.Deleting,
        Definition = "Delete Image"
    )]
    [Authorize(AuthenticationSchemes = "Auth")]
    public async Task<IActionResult> DeleteImage(string imageId)
    {
        await _imageFileService.DeleteImage(imageId);

        return Ok();
    }

    [HttpGet("[action]/{definition}")]
    public async Task<IActionResult> GetImagesByDefinition([FromRoute] string definition)
    {
        var response = await _imageFileService.GetImagesByDefinition(definition);

        return Ok(response);
    }

    [HttpPost("[action]")]
    [Authorize(AuthenticationSchemes = "Auth")]
    [AuthorizeDefinition(
        Menu = "ImageFile",
        ActionType = ActionType.Deleting,
        Definition = "Update Order Definition Images"
    )]
    public async Task<IActionResult> UpdateOrderDefinitionImages(
        [FromBody] UpdateOrderDefinitionImagesRequestDTO model
    )
    {
        await _imageFileService.UpdateOrderDefinitionImages(model);

        return Ok();
    }
}
