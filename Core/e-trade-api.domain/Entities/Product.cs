using e_trade_api.domain.Entities.Common;

namespace e_trade_api.domain.Entities
{
    public class Product : BaseEntity
    {
        public int Name { get; set; }
        public int Stock { get; set; }
        public long Price { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
