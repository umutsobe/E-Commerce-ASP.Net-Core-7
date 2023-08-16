using e_trade_api.domain.Entities;
using e_trade_api.domain.Entities.Common;

namespace e_trade_api.domain;

public class CompletedOrder : BaseEntity
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
}
