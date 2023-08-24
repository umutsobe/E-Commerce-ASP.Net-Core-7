using e_trade_api.domain.Entities.Common;

namespace e_trade_api.domain;

public class Adress : BaseEntity
{
    public string Definition { get; set; }
    public string FullAdress { get; set; }
    public AppUser User { get; set; }
    public string UserId { get; set; }
}
