using Microsoft.Extensions.Configuration;

namespace e_trade_api.application;

public static class MyConfigurationManager
{
    public static ConfigurationManager ConfigurationManager()
    {
        ConfigurationManager configurationManager = new();

        try //burada hata alırsak catch bloğuna git. publishteyken hata alabiliriz çünkü orada her şey aynı dizinde. buradaki dizin işleminde hata alacağız. burada hata almıyorsak development sürecindeyizdir zaten
        {
            configurationManager.SetBasePath(
                Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/e-trade-api.API")
            );
            configurationManager.AddJsonFile("appsettings.json");
            return configurationManager;
        }
        catch //yukarıda hata alıyorsak publishdeyizdir zaten
        {
            configurationManager.AddJsonFile("appsettings.Production.json");
            return configurationManager;
        }
    }

    public static string GetClientUrl() //ok
    {
        return ConfigurationManager().GetSection("AngularClientUrl").Get<string>();
    }

    public static string GetConnectionString() //ok
    {
        return ConfigurationManager().GetSection("ConnectionStrings:SqlServer").Get<string>();
    }

    public static string GetAzureStorageConnectionString() //ok
    {
        return ConfigurationManager().GetSection("Storage:Azure").Get<string>();
    }

    public static string GetBaseAzureStorageUrl()
    {
        return ConfigurationManager().GetSection("BaseStorageUrl").Get<string>();
    }

    public static MailConfiguraiton GetMailModel() //ok
    {
        return ConfigurationManager().GetSection("Mail").Get<MailConfiguraiton>();
    }

    public static TokenConfiguration GetTokenModel() //ok
    {
        return ConfigurationManager().GetSection("Token").Get<TokenConfiguration>();
    }
}

// GetAzureStorageConnectionString
// GetBaseAzureStorageUrl
// GetMailModel
// GetTokenModel

public class MailConfiguraiton
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string EmailHeader { get; set; }
}

public class TokenConfiguration
{
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string SecurityKey { get; set; }
}
