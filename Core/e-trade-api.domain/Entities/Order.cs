using e_trade_api.domain.Entities.Common;

namespace e_trade_api.domain.Entities
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; } //userid guid değil string gelecek, bunu identityde seçmiştik
        public AppUser User { get; set; }
        public string Description { get; set; }
        public string Adress { get; set; }
        public string OrderCode { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public CompletedOrder CompletedOrder { get; set; }
    }
}
