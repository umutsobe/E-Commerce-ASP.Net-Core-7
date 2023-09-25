namespace e_trade_api.application;

public class UpdateOrderDefinitionImagesRequestDTO
{
    public string Definition { get; set; }
    public List<UpdateOrderDefinitionImage> Images { get; set; }
}

public class UpdateOrderDefinitionImage
{
    public string ImageId { get; set; }
    public int Order { get; set; }
}
