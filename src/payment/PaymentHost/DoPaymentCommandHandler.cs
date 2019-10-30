using System;
using System.Threading.Tasks;
using MassTransit;
using Payment.Messages;

namespace CoolBrains.PaymentHost
{
    public class DoPaymentCommandHandler : IConsumer<DoPaymentCommand>
    {
        public Task Consume(ConsumeContext<DoPaymentCommand> context)
        {
            Console.WriteLine($"2. Payment has collected with coreelation id");
            return Task.CompletedTask;
        }
    }
}
