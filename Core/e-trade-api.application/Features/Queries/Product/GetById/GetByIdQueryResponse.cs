using e_trade_api.domain.Entities;

namespace e_trade_api.application;

public class GetByIdQueryResponse
{
    public string Name { get; set; }
    public int Stock { get; set; }
    public float Price { get; set; }
}
