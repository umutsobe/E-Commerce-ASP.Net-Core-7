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
    readonly IProductService _productService;

    public ProductControllers(IMediator mediator, IProductService productService)
    {
        _mediator = mediator;
        _productService = productService;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllProducts(
        [FromQuery] GetAllProductQueryRequest getAllProductQueryRequest
    ) // eğer api üzerinden bir şey geriye döndürüyorsak iactionresult döndürmek zorundayız
    {
        GetAllProductQueryResponse response = await _mediator.Send(getAllProductQueryRequest);
        return Ok(response);
    }

    [HttpGet("[action]/{Id}")]
    public async Task<IActionResult> GetProductById(
        [FromRoute] GetByIdQueryRequest getByIdQueryRequest
    ) // eğer api üzerinden bir şey geriye döndürüyorsak iactionresult döndürmek zorundayız
    {
        GetByIdQueryResponse response = await _mediator.Send(getByIdQueryRequest);
        return Ok(response);
    }

    [HttpPost("[action]")]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Products,
        ActionType = ActionType.Writing,
        Definition = "Create Product"
    )]
    public async Task<IActionResult> CreateProduct(
        CreateProductCommandRequest createProductCommandRequest
    )
    {
        CreateProductCommandResponse createProductCommandResponse = await _mediator.Send(
            createProductCommandRequest
        );
        return Ok(createProductCommandResponse);
    }

    [HttpPut("[action]")]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Products,
        ActionType = ActionType.Updating,
        Definition = "Update Product"
    )]
    public async Task<IActionResult> UpdateProduct(
        [FromBody] UpdateProductCommandRequest updateProductCommandRequest
    )
    {
        UpdateProductCommandResponse response = await _mediator.Send(updateProductCommandRequest);

        return Ok();
    }

    [HttpDelete("[action]/{Id}")]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Products,
        ActionType = ActionType.Deleting,
        Definition = "Delete Product"
    )]
    public async Task<IActionResult> DeleteProductById(
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
    public async Task<IActionResult> UploadProductImage(
        [FromQuery] UploadProductImageCommandRequest uploadProductImageCommandRequest
    )
    {
        uploadProductImageCommandRequest.Files = Request.Form.Files; //application katmanında gelen dosyaları yakalayamadık. o yüzden böyle yazdık
        await _mediator.Send(uploadProductImageCommandRequest);
        return Ok();
    }

    [HttpGet("[action]/{Id}")]
    // [Authorize(AuthenticationSchemes = "Admin")]
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

    [HttpPost("[action]")]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Products,
        ActionType = ActionType.Writing,
        Definition = "Assign Category To Product"
    )]
    [Authorize(AuthenticationSchemes = "Admin")]
    public async Task<IActionResult> AssignCategoryToProduct(
        AssignCategoryToProductRequestDTO model
    )
    {
        await _productService.AssignCategoryToProduct(model);
        return Ok();
    }

    [HttpGet("[action]/{productId}")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Products,
        ActionType = ActionType.Reading,
        Definition = "Get Categories By Product"
    )]
    [Authorize(AuthenticationSchemes = "Admin")]
    public async Task<IActionResult> GetCategoriesByProduct([FromRoute] string productId)
    {
        List<string> response = await _productService.GetCategoriesByProduct(productId);

        return Ok(response);
    }

    [HttpPost("[action]")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Products,
        ActionType = ActionType.Writing,
        Definition = "Add Products To Category"
    )]
    [Authorize(AuthenticationSchemes = "Admin")]
    public async Task<IActionResult> AddProductsToCategory(
        [FromBody] AddProductsToCategoryRequestDTO model
    )
    {
        await _productService.AddProductsToCategory(model);
        return Ok();
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetProductsByFilter( //ok
        [FromQuery] GetProductsByFilterDTO model
    )
    {
        GetAllProductsResponseDTO response = await _productService.GetProductsByFilter(model);

        return Ok(response);
    }
}
