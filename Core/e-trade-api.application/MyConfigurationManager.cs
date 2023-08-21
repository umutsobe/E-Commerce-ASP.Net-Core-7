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

    public static string GetClientUrl()
    {
        return ConfigurationManager().GetSection("AngularClientUrl").Get<string>();
    }

    public static string GetConnectionString()
    {
        return ConfigurationManager().GetSection("ConnectionStrings:SqlServer").Get<string>();
    }
}
