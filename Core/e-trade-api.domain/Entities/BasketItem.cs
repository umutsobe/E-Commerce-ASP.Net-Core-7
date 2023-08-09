using e_trade_api.domain.Entities;
using e_trade_api.domain.Entities.Common;

namespace e_trade_api.domain;

public class BasketItem : BaseEntity
{
    public Basket Basket { get; set; }
    public Guid BasketId { get; set; }

    public Product Product { get; set; }
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
}
