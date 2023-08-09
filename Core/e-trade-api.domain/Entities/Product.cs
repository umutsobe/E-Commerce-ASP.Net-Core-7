using e_trade_api.domain.Entities.Common;

namespace e_trade_api.domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
        public ICollection<ProductImageFile> ProductImageFiles { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; } // bir ürün kaç defa sepete eklendi bilgisini isteyebiliriz
        public ICollection<OrderItem> OrderItems { get; set; } // bir ürün kaç defa satın alındı bilgisini isteyebiliriz
    }
}
