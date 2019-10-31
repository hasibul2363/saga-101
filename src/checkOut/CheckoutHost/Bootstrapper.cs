using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoolBrains.CheckoutHost
{
    public class Bootstrapper
    {
        public IServiceProvider ServiceProvider { get; set; }
        public IConfiguration Configuration { get; set; }
        public void Boot()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();


            RegisterInstances();
        }


        private void RegisterInstances()
        {
            var serviceCollection = new ServiceCollection();


            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
