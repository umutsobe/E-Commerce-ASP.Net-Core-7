using e_trade_api.domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace e_trade_api.domain;

public class AppUser : IdentityUser<string>
{
    public string Name { get; set; }
    public Basket Basket { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<Adress> Adresses { get; set; }
}
