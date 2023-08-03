using e_trade_api.domain.Entities;

namespace e_trade_api.domain;

public class ProductImageFile : File
{
    public bool Showcase { get; set; }
    public ICollection<Product> Products { get; set; }
}
