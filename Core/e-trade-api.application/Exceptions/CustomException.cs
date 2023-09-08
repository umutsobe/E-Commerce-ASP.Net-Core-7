namespace e_trade_api.application;

public class CustomException : Exception
{
    public CustomException()
        : base() { }

    public CustomException(string message)
        : base(message) { }
}
