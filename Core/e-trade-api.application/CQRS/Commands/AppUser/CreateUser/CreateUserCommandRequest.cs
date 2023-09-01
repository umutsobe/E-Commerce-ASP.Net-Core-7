using MediatR;

namespace e_trade_api.application;

public class CreateUserCommandRequest : IRequest<CreateUserCommandResponse>
{
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string RepeatPassword { get; set; }
}



//   name: string;
//   email: string;
//   password: string;
//   repeatPassword: string;
