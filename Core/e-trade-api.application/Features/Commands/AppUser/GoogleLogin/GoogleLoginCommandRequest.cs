using MediatR;

namespace e_trade_api.application;

public class GoogleLoginCommandRequest : IRequest<GoogleLoginCommandResponse>
{
    public string Id { get; set; }
    public string IdToken { get; set; }
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhotoUrl { get; set; }
    public string Provider { get; set; }
}



// email
// :
// "umuttsobeksk@gmail.com"
// firstName
// :
// "Umut Söbe"
// id
// :
// "114300448046959112322"
// idToken
// :
// lastName
// :
// undefined
// name
// :
// "Umut Söbe"
// photoUrl
// :
// "https://lh3.googleusercontent.com/a/AAcHTtfTPV04FX436e2qVU7RS3HQ3w5h8L8pdchGwZi1zriCPg=s96-c"
// provider
// :
// "GOOGLE"
