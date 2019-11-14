using System;
using System.Threading.Tasks;
using MassTransit;
using Payment.Messages;

namespace CoolBrains.PaymentHost
{
    public class DoPaymentCommandHandler : IConsumer<DoPaymentCommand>
    {
        public async Task Consume(ConsumeContext<DoPaymentCommand> context)
        {
            Console.WriteLine($"2. Payment has collected with coreelation id");
            await context.Publish(new PaymentPerformed
            {
                Amount = context.Message.Amount, CorrelationId = context.Message.CorrelationId,
                CheckoutId = context.Message.CheckoutId
            });
            
        }
    }
}
