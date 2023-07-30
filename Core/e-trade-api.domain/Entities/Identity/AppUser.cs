using Microsoft.AspNetCore.Identity;

namespace e_trade_api.domain;

public class AppUser : IdentityUser<string>
{
    public string Name { get; set; }
}
