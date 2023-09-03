using System.ComponentModel.DataAnnotations.Schema;
using e_trade_api.domain.Entities;
using e_trade_api.domain.Entities.Common;

namespace e_trade_api.domain;

public class ProductImageFile : BaseEntity
{
    public string FileName { get; set; }
    public string Path { get; set; }
    public string Storage { get; set; }

    [NotMapped]
    public override DateTime UpdatedDate
    {
        get => base.UpdatedDate;
        set => base.UpdatedDate = value;
    }
    public bool Showcase { get; set; }
    public Product Product { get; set; }
    public Guid ProductId { get; set; }
}
