using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

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
