using e_trade_api.domain.Entities;
using e_trade_api.domain.Entities.Common;

namespace e_trade_api.domain;

public class Basket : BaseEntity //id base entity üzeirnden gelecek
{
    public string UserId { get; set; } //userid guid değil string gelecek, bunu identityde seçmiştik

    public AppUser User { get; set; }
    public ICollection<BasketItem> BasketItems { get; set; }
}
