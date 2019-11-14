using System;
using System.Threading.Tasks;
using Bus.Host;
using MassTransit;
using MassTransit.Saga;
using Microsoft.Extensions.DependencyInjection;
using Payment.Messages;

namespace CoolBrains.PaymentHost
{
    class Program
    {
        private static IServiceCollection _serviceCollection = new ServiceCollection();
        static async Task Main(string[] args)
        {

            Register();
            var bootstrapper = new BusBootstrapper(_serviceCollection, "saga-101_payment", c =>
            {
                c.Consumer<DoPaymentCommandHandler>();

            });

            await bootstrapper.Start();

            Console.WriteLine("Hello World!");
        }
        private static void Register()
        {
            _serviceCollection.AddScoped<DoPaymentCommandHandler>();
            _serviceCollection.AddScoped<IConsumer<DoPaymentCommand>, DoPaymentCommandHandler>();

        }
    }
}
