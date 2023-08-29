using System.ComponentModel.DataAnnotations;
using e_trade_api.domain.Entities;
using e_trade_api.domain.Entities.Common;

namespace e_trade_api.domain;

public class ProductRating : BaseEntity
{
    public Product Product { get; set; }
    public Guid ProductId { get; set; }
    public AppUser User { get; set; }
    public string UserId { get; set; }

    [Range(1, 5)]
    public int Star { get; set; }
    public string Comment { get; set; }
}
