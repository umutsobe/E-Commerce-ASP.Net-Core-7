using e_trade_api.domain.Entities.Common;

namespace e_trade_api.domain;

public class Menu : BaseEntity //menu = controller'larımız. productController, OrderController ...
{
    public string Name { get; set; }

    public ICollection<Endpoint> Endpoints { get; set; }
}
