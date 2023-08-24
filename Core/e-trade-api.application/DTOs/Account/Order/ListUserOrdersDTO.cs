namespace e_trade_api.application;

public class ListUserOrdersDTO
{
    public string OrderCode { get; set; }
    public string CreatedDate { get; set; }
    public float TotalPrice { get; set; }
    public string Adress { get; set; }
    public List<ListUserOrderItemsDTO> OrderItems { get; set; }
}
