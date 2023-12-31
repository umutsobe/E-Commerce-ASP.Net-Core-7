using Microsoft.AspNetCore.Identity;

namespace e_trade_api.domain;

public class AppRole : IdentityRole<string>
{
    public ICollection<Endpoint> Endpoints { get; set; }
}
