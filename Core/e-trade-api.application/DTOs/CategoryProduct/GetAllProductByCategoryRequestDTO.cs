namespace e_trade_api.application;

public class GetAllProductByCategoryRequestDTO
{
    public string CategoryName { get; set; }
    public int Page { get; set; } = 0;
    public int Size { get; set; } = 5;
}
