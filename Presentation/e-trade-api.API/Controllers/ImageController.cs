using e_trade_api.application;
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
    public async Task<IActionResult> UploadImage([FromQuery] UploadImageRequestDTO model)
    {
        model.Files = Request.Form.Files; //dosyaları bodyi querystring veya diğer şekilde yakalayamayız
        await _imageFileService.UploadImage(model);

        return Ok();
    }

    [HttpDelete("[action]/{imageId}")]
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
}
