using Microsoft.Extensions.Configuration;

using System;

namespace RemoteMonitor.Library.Core
{
    public static class Extensions
    {
        public static IConfigurationRoot BuildConfiguraion(this string functionAppDirectory)
        {
            var config = new ConfigurationBuilder()
                                .SetBasePath(functionAppDirectory)
                                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                                .AddEnvironmentVariables()
                                .Build();
            return config;
        }
    }
}
