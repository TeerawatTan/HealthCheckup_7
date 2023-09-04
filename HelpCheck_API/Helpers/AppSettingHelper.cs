using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Helpers
{
    public class AppSettingHelper
    {
        static public IConfigurationRoot Configuration { get; set; }

        public AppSettingHelper()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public string GetConfiguration(string key)
        {
            return Configuration[key];
        }

        public string GetConnectionString(string name)
        {
            string _result = Configuration["ConnectionStrings:" + name];

            return _result;
        }
    }
}
