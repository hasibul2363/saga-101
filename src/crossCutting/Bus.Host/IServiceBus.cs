using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MassTransit.RabbitMqTransport;

namespace Bus.Host
{
    public interface IServiceBus
    {
        Task Send<T>(T message);
        Task Publish<T>(T message);
    }

    public interface IServiceBusHost
    {
        Task Listen(Action<IRabbitMqReceiveEndpointConfigurator> configure);
    }
}
