namespace e_trade_api.application;

public class GetAllCategoriesDTO
{
    public int totalCategoryCount { get; set; }
    public List<CategoryDTO> Categories { get; set; }
}
