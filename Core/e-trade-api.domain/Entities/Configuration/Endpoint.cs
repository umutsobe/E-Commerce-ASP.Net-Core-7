using e_trade_api.domain.Entities.Common;

namespace e_trade_api.domain;

public class Endpoint : BaseEntity
{
    public Endpoint()
    {
        Roles = new HashSet<AppRole>();
    }

    public string ActionType { get; set; }
    public string HttpType { get; set; }
    public string Definition { get; set; }
    public string Code { get; set; }
    public Guid MenuId { get; set; }

    public Menu Menu { get; set; }
    public ICollection<AppRole> Roles { get; set; }
}
