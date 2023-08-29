namespace e_trade_api.application;

public class CreateProductDTO
{
    public string Name { get; set; }
    public int Stock { get; set; }
    public float Price { get; set; }
    public bool isActive { get; set; }
    public string Description { get; set; }
    public string[] CategoryNames { get; set; }
}
