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

    [HttpPost("[action]")]
    [Authorize(AuthenticationSchemes = "Auth")]
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
    [Authorize(AuthenticationSchemes = "Auth")]
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
    [Authorize(AuthenticationSchemes = "Auth")]
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

    [HttpDelete("[action]/{ProductId}")] //alttaki isimle buradaki isim aynı olmak zorunda. buradaki productId, pareametredeki productId
    [Authorize(AuthenticationSchemes = "Auth")]
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

    [HttpPost("[action]")]
    [Authorize(AuthenticationSchemes = "Auth")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Products,
        ActionType = ActionType.Writing,
        Definition = "Assign Category To Product"
    )]
    [Authorize(AuthenticationSchemes = "Auth")]
    public async Task<IActionResult> AssignCategoryToProduct(
        AssignCategoryToProductRequestDTO model
    )
    {
        await _productService.AssignCategoryToProduct(model);
        return Ok();
    }

    [HttpGet("[action]/{productId}")]
    // [AuthorizeDefinition(
    //     Menu = AuthorizeDefinitionConstants.Products,
    //     ActionType = ActionType.Reading,
    //     Definition = "Get Categories By Product"
    // )]
    // [Authorize(AuthenticationSchemes = "Auth")]
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
    [Authorize(AuthenticationSchemes = "Auth")]
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

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = "Auth")]
    public async Task<IActionResult> QuickCreateProduct()
    {
        await _productService.QuickCreateProduct();
        return Ok();
    }

    [HttpGet("[action]/{urlId}")]
    public async Task<IActionResult> GetProductByUrlIdRequest([FromRoute] string urlId)
    {
        GetProductByIdDTO response = await _productService.GetProductByUrlId(urlId);
        return Ok(response);
    }

    //adminlik ekle
    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllProductsAdmin(
        [FromQuery] GetAllProductAdminQueryRequest getAllProductQueryRequest
    )
    {
        GetAllProductAdminQueryResponse response = await _mediator.Send(getAllProductQueryRequest);
        return Ok(response);
    }

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = "Auth")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Products,
        ActionType = ActionType.Updating,
        Definition = "Change Showcase Image"
    )]
    public async Task<IActionResult> ChangeShowcaseImage(
        [FromQuery] ChangeShowCaseImageRequestDTO requestDTO
    )
    {
        await _productService.ChangeShowcaseImage(requestDTO);
        return Ok();
    }

    [HttpPost("[action]")]
    [Authorize(AuthenticationSchemes = "Auth")]
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
    public async Task<IActionResult> GetProductImages(
        [FromRoute] GetProductImageQueryRequest getProductImageQueryRequest
    )
    {
        List<GetProductImageQueryResponse> response = await _mediator.Send(
            getProductImageQueryRequest
        );
        return Ok(response);
    }

    [HttpPut("[action]")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Products,
        ActionType = ActionType.Updating,
        Definition = "Add Stock"
    )]
    [Authorize(AuthenticationSchemes = "Auth")]
    public async Task<IActionResult> AddStock(AddStockRequestDTO model)
    {
        await _productService.AddStock(model);
        return Ok();
    }

    [HttpPut("[action]/{productId}")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Products,
        ActionType = ActionType.Updating,
        Definition = "Deactivate Product"
    )]
    [Authorize(AuthenticationSchemes = "Auth")]
    public async Task<IActionResult> DeactivateProduct(string productId)
    {
        await _productService.DeactivateProduct(productId);
        return Ok();
    }

    [HttpPut("[action]/{productId}")]
    [Authorize(AuthenticationSchemes = "Auth")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Products,
        ActionType = ActionType.Updating,
        Definition = "Activate Product"
    )]
    public async Task<IActionResult> ActivateProduct(string productId)
    {
        await _productService.ActivateProduct(productId);
        return Ok();
    }
}
