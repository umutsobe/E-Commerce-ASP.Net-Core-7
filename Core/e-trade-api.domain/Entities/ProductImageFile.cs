using e_trade_api.domain.Entities;

namespace e_trade_api.domain;

public class ProductImageFile : File
{
    public ICollection<Product> Products { get; set; }
}
