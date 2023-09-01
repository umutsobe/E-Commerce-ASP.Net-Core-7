using MediatR;

namespace e_trade_api.application;

public class ChangeShowcaseImageCommandRequest : IRequest<ChangeShowcaseImageCommandResponse>
{
    public string ImageId { get; set; }
    public string ProductId { get; set; }
}
