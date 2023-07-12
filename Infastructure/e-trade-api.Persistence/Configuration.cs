using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_trade_api.Persistence
{
    static class Configuration
    {
        public static string GetConnectionString()
        {
            ConfigurationManager configurationManager = new ConfigurationManager();
            configurationManager.SetBasePath(
                Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/e-trade-api.API")
            );
            configurationManager.AddJsonFile("appsettings.json");

            return configurationManager.GetConnectionString("SqlServer");
        }
    }
}
