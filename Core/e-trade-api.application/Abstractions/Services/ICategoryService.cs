namespace e_trade_api.application;

public interface ICategoryService
{
    Task CreateCategory(string name);
    Task<GetAllCategoriesDTO> GetAllCategories(GetAllCategoriesRequestDTO model);
    Task DeleteCategory(string id);
    Task UpdateCategoryName(UpdateCategoryNameDTO model);
}
