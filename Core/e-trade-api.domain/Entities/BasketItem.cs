using System.ComponentModel.DataAnnotations;
using e_trade_api.domain.Entities;
using e_trade_api.domain.Entities.Common;

namespace e_trade_api.domain;

public class BasketItem : BaseEntity
{
    public Basket Basket { get; set; }
    public Guid BasketId { get; set; }

    public Product Product { get; set; }
    public Guid ProductId { get; set; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}
