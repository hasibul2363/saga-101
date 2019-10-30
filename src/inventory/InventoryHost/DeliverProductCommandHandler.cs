using System;
using System.Threading.Tasks;
using Inventory.Messages;
using MassTransit;

namespace CoolBrains.InventoryHost
{
    public class DeliverProductCommandHandler : IConsumer<DeliverProductCommand>
    {
        public Task Consume(ConsumeContext<DeliverProductCommand> context)
        {
            Console.WriteLine($"3. Item has delivered to owner with coreelation id {context.Message.CorrelationId}");
            return Task.CompletedTask;

        }
    }
}
