using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace CoolBrains.CheckoutHost
{
    public class CreatePendingCheckoutCommandHandler: IConsumer<CreatePendingCheckoutCommand>
    {
        public async Task Consume(ConsumeContext<CreatePendingCheckoutCommand> context)
        {
            Console.WriteLine($"1. Checkout Handler: Checkout has created with id {context.Message.CheckoutId} and amount {context.Message.Amount}");

            await context.Publish(new CheckoutCreated
            {
                CorrelationId = context.Message.CorrelationId,
                ProductId = context.Message.ProductId,
                CheckoutId = context.Message.CheckoutId,
                Amount = context.Message.Amount,
                CheckoutBy = context.Message.CheckoutBy
            });
        }
    }
}
