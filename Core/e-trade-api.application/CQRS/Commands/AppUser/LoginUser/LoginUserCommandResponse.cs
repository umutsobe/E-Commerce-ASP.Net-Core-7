namespace e_trade_api.application;

public class LoginUserCommandResponse { }

public class LoginUserSuccessCommandResponse : LoginUserCommandResponse
{
    public Token Token { get; set; }
}

public class LoginUserErrorCommandResponse : LoginUserCommandResponse
{
    public string Message { get; set; }
}

public class LoginUserSuccessButEmailNotConfirmed : LoginUserCommandResponse
{
    public string AuthMessage { get; set; }
    public string UserId { get; set; }
}
