using Microsoft.Extensions.Configuration;

namespace e_trade_api.application;

public static class MyConfigurationManager
{
    public static ConfigurationManager ConfigurationManager()
    {
        ConfigurationManager configurationManager = new();

        ProjectStatus projectStatus = ProjectStatus.Development;

        if (projectStatus == ProjectStatus.Development)
        {
            configurationManager.SetBasePath(
                Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/e-trade-api.API")
            );
            configurationManager.AddJsonFile("appsettings.json");
            return configurationManager;
        }
        else if (projectStatus == ProjectStatus.UpdateServerOnLocal)
        {
            configurationManager.SetBasePath(
                Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/e-trade-api.API")
            );
            configurationManager.AddJsonFile("appsettings.Production.json");
            return configurationManager;
        }
        else if (projectStatus == ProjectStatus.Production)
        {
            configurationManager.AddJsonFile("appsettings.Production.json");
            return configurationManager;
        }

        throw new Exception("MyConfigurationManager uzerinde hata");
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
