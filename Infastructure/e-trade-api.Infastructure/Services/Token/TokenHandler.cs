using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using e_trade_api.application;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace e_trade_api.Infastructure;

public class TokenHandler : ITokenHandler
{
    readonly IConfiguration _configuration;
    readonly IBasketService _basketService;

    public TokenHandler(IConfiguration configuration, IBasketService basketService)
    {
        _configuration = configuration;
        _basketService = basketService;
    }

    public async Task<Token> CreateAccessToken(int minute, string userId)
    {
        Token token = new();

        //Security Key'in simetriğini alıyoruz.
        SymmetricSecurityKey securityKey =
            new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

        //Şifrelenmiş kimliği oluşturuyoruz.
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        //Oluşturulacak token ayarlarını veriyoruz.
        token.Expiration = DateTime.UtcNow.AddMinutes(minute); // süreye dikkat

        Claim userIdClaim = new("userId", userId);

        string basketId = await _basketService.GetBasketId(userId);
        Claim basketIdClaim = new("basketId", basketId);

        JwtSecurityToken securityToken =
            new(
                audience: _configuration["Token:Audience"],
                issuer: _configuration["Token:Issuer"],
                expires: token.Expiration,
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials,
                claims: new[] { userIdClaim, basketIdClaim }
            );

        //Token oluşturucu sınıfından bir örnek alalım.
        JwtSecurityTokenHandler tokenHandler = new();
        token.AccessToken = tokenHandler.WriteToken(securityToken);
        return token;
    }
}
