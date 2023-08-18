namespace e_trade_api.application;

public class AuthorizeDefinitionAttribute : Attribute
{
    public string Menu { get; set; }
    public string Definition { get; set; }
    public ActionType ActionType { get; set; }
}
