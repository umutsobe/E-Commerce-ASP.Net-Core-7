namespace e_trade_api.application;

public class AssignCategoryToProductRequestDTO
{
    public string ProductId { get; set; }
    public List<string> CategoryNames { get; set; }
}
