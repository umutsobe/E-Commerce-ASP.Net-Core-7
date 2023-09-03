using System.ComponentModel.DataAnnotations;
using e_trade_api.domain.Entities.Common;

namespace e_trade_api.domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Range(0, float.MaxValue)]
        public float Price { get; set; }
        public bool isActive { get; set; }
        public string Description { get; set; }
        public int TotalBasketAdded { get; set; } = 0;
        public int TotalOrderNumber { get; set; } = 0;
        public string Url { get; set; }
        public ICollection<ProductImageFile> ProductImageFiles { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; } // bir ürün kaç defa sepete eklendi bilgisini isteyebiliriz
        public ICollection<OrderItem> OrderItems { get; set; } // bir ürün kaç defa satın alındı bilgisini isteyebiliriz
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<ProductRating> ProductRatings { get; set; }
    }
}
