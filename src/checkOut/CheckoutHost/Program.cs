using System;
using System.IO;
using System.Threading.Tasks;
using Bus.Host;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Saga;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoolBrains.CheckoutHost
{
    class Program
    {
        private static IServiceCollection _serviceCollection = new ServiceCollection();
        static async Task Main(string[] args)
        {

            Register();
            var bootstrapper = new BusBootstrapper(_serviceCollection, "saga-101_checkout", c =>
            {
                c.Consumer<CreatePendingCheckoutCommandHandler>();
                //c.Consumer(()=> new CreatePendingCheckoutCommandHandler());


                var stateMachine = new CheckoutStateMachine();
                var repo = new InMemorySagaRepository<CheckoutState>();
                c.StateMachineSaga(stateMachine, repo);
                


            });

            await bootstrapper.Start();
            var bus = bootstrapper.GetBus();
            var sender = await bus.GetSendEndpoint(new Uri("rabbitmq://localhost/saga-101_checkout"));

            await sender.Send(new CreatePendingCheckoutCommand
            {
                CorrelationId = Guid.NewGuid(),
                CheckoutId = Guid.NewGuid(),
                Amount = 250,
                ProductId = Guid.NewGuid(),
                CheckoutBy = Guid.NewGuid()
            });

            Console.WriteLine("Hello World!");
        }


        private static void Register()
        {
            _serviceCollection.AddScoped<CreatePendingCheckoutCommandHandler>();
            _serviceCollection.AddScoped<IConsumer<CreatePendingCheckoutCommand>, CreatePendingCheckoutCommandHandler>();
            
        }
    }
}
