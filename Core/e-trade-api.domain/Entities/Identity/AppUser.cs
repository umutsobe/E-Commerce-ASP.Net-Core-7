using e_trade_api.domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace e_trade_api.domain;

public class AppUser : IdentityUser<string>
{
    public string Name { get; set; }
    public Basket Basket { get; set; }

    //two factor auth
    public int CodeAttemptCount { get; set; } // Kod deneme sayısı
    public DateTime? LastCodeAttemptTime { get; set; } // Son deneme tarihi
    public string NewEmailControl { get; set; } //yeni mail adresini depolamak için. bu olmazsa kod doğrulanırken ilk adımdakiyle aynı email mi olacağını bilemeyiz

    //
    public ICollection<Order> Orders { get; set; }
    public ICollection<Adress> Adresses { get; set; }
    public ICollection<ProductRating> ProductRatings { get; set; }
    public ICollection<TwoFactorAuthentication> TwoFactorAuthentications { get; set; }
}
