using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;
using MassTransit.Saga;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bus.Host
{
    public class BusBootstrapper
    {

        public BusBootstrapper(IServiceCollection serviceCollection, string queue, Action<IRabbitMqReceiveEndpointConfigurator> configure)
        {
            _serviceCollection = serviceCollection;
            BuildConfiguration();
            Boot(queue, configure);
        }

        private IConfiguration _configuration;
        private IServiceCollection _serviceCollection;
        private RabbitSetting _rabbitSetting;
        private IBusControl _busControl;
        private IBus _bus;


        private void BuildConfiguration()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            _serviceCollection.Configure<RabbitSetting>(_configuration.GetSection("rabbitSetting"));
            
        }
        private IBusControl CreateBust(string queue, Action<IRabbitMqReceiveEndpointConfigurator> configure)
        {
            var builder = _serviceCollection.BuildServiceProvider();
            _rabbitSetting = builder.GetService<IOptions<RabbitSetting>>().Value;

    
            var bus = MassTransit.Bus.Factory.CreateUsingRabbitMq(configurator =>
            {
                var host = configurator.Host(new Uri(_rabbitSetting.Host), h =>
                {
                    h.Username(_rabbitSetting.UserId);
                    h.Password(_rabbitSetting.Password);
                });

                //configurator.ReceiveEndpoint(host, queue, c =>
                //{
                //    c.ConfigureConsumer(_serviceCollection.BuildServiceProvider());
                //});
                configurator.ReceiveEndpoint(host, queue, configure);

            });
            
            return bus;
        }
        private void Boot(string queue, Action<IRabbitMqReceiveEndpointConfigurator> configure)
        {
            _busControl = CreateBust(queue,configure);
            _bus = _busControl;
            _serviceCollection.AddSingleton<IBus>(_bus);
        }

        public IBusControl GetBus()
        {
            return _busControl;
        }

        public Task Start()
        {
            return _busControl.StartAsync();
        }

        public Task Stop()
        {
            return _busControl.StopAsync();
        }


    }
}
