using System.ComponentModel.DataAnnotations;
using e_trade_api.domain.Entities;
using e_trade_api.domain.Entities.Common;

namespace e_trade_api.domain;

public class OrderItem : BaseEntity
{
    public Order Order { get; set; }
    public Guid OrderId { get; set; }
    public Product Product { get; set; }
    public Guid ProductId { get; set; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(0, float.MaxValue)]
    public float Price { get; set; } //orderın sipariş verildiği tarihteki fiyatını isteyebiliriz. zamlanabilir
}
