using Microsoft.Extensions.Configuration;

namespace PERI.Agenda.Test
{
    public class TestHelper
    {
        public static void GetApplicationConfiguration()
        {
            Core.Setting.Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
