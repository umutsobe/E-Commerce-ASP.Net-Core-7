using e_trade_api.domain.Entities.Common;

namespace e_trade_api.domain.Entities
{
    public class Order : BaseEntity
    {
        public string? Description { get; set; }
        public string? Adress { get; set; }
        public ICollection<Product> Products { get; set; }
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
    }
}
