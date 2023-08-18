using e_trade_api.application;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_trade_api.API;

[Route("api/[controller]")]
[ApiController]
public class ProductControllers : ControllerBase
{
    readonly IMediator _mediator;

    public ProductControllers(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] GetAllProductQueryRequest getAllProductQueryRequest
    ) // eğer api üzerinden bir şey geriye döndürüyorsak iactionresult döndürmek zorundayız
    {
        GetAllProductQueryResponse response = await _mediator.Send(getAllProductQueryRequest);
        return Ok(response);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> Get([FromRoute] GetByIdQueryRequest getByIdQueryRequest) // eğer api üzerinden bir şey geriye döndürüyorsak iactionresult döndürmek zorundayız
    {
        GetByIdQueryResponse response = await _mediator.Send(getByIdQueryRequest);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Products,
        ActionType = ActionType.Writing,
        Definition = "Create Product"
    )]
    public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
    {
        CreateProductCommandResponse createProductCommandResponse = await _mediator.Send(
            createProductCommandRequest
        );
        return StatusCode(201);
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Products,
        ActionType = ActionType.Updating,
        Definition = "Update Product"
    )]
    public async Task<IActionResult> Put(
        [FromBody] UpdateProductCommandRequest updateProductCommandRequest
    )
    {
        UpdateProductCommandResponse response = await _mediator.Send(updateProductCommandRequest);

        return Ok();
    }

    [HttpDelete("{Id}")]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Products,
        ActionType = ActionType.Deleting,
        Definition = "Delete Product"
    )]
    public async Task<IActionResult> Delete(
        [FromRoute] DeleteProductByIdCommandRequest deleteProductByIdCommandRequest
    )
    {
        DeleteProductByIdCommandResponse response = await _mediator.Send(
            deleteProductByIdCommandRequest
        );

        return Ok(response);
    }

    [HttpPost("[action]")]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Products,
        ActionType = ActionType.Writing,
        Definition = "Upload Product File"
    )]
    public async Task<IActionResult> Upload(
        [FromQuery] UploadProductImageCommandRequest uploadProductImageCommandRequest
    )
    {
        uploadProductImageCommandRequest.Files = Request.Form.Files; //application katmanında gelen dosyaları yakalayamadık. o yüzden böyle yazdık
        await _mediator.Send(uploadProductImageCommandRequest);
        return Ok();
    }

    [HttpGet("[action]/{Id}")]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Products,
        ActionType = ActionType.Reading,
        Definition = "Get Products Images"
    )]
    public async Task<IActionResult> GetProductImages(
        [FromRoute] GetProductImageQueryRequest getProductImageQueryRequest
    )
    {
        List<GetProductImageQueryResponse> response = await _mediator.Send(
            getProductImageQueryRequest
        );
        return Ok(response);
    }

    [HttpDelete("[action]/{ProductId}")] //alttaki isimle buradaki isim aynı olmak zorunda. buradaki productId, pareametredeki productId
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Products,
        ActionType = ActionType.Deleting,
        Definition = "Delete Product Image"
    )]
    public async Task<IActionResult> DeleteImage(
        [FromRoute] DeleteProductImageCommandRequest deleteProductImageCommandRequest,
        [FromQuery] string imageId
    ) //productId parametre olarak geliyor, imageId ise queryStringden geliyor
    {
        deleteProductImageCommandRequest.ImageId = imageId;
        DeleteProductImageCommandResponse response = await _mediator.Send(
            deleteProductImageCommandRequest
        );
        return Ok(response);
    }

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Products,
        ActionType = ActionType.Updating,
        Definition = "Change Showcase Image"
    )]
    public async Task<IActionResult> ChangeShowcaseImage(
        [FromQuery] ChangeShowcaseImageCommandRequest changeShowcaseImageCommandRequest
    )
    {
        ChangeShowcaseImageCommandResponse response = await _mediator.Send(
            changeShowcaseImageCommandRequest
        );
        return Ok(response);
    }
}
