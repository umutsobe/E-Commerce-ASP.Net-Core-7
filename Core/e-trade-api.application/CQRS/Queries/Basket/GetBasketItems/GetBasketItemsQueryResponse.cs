namespace e_trade_api.application;

public class GetBasketItemsQueryResponse
{
    public string BasketItemId { get; set; }
    public string ProductId { get; set; }
    public string Name { get; set; }
    public float Price { get; set; }
    public int Quantity { get; set; }
}
