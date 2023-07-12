using e_trade_api.domain.Entities.Common;

namespace e_trade_api.domain.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
