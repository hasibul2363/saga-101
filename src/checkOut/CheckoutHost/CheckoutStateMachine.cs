using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Automatonymous;
using Automatonymous.Binders;
using Inventory.Messages;
using Payment.Messages;

namespace CoolBrains.CheckoutHost
{
    public class CheckoutStateMachine : MassTransitStateMachine<CheckoutState>
    {
        public CheckoutStateMachine()
        {
            InstanceState(p => p.State);
            Correlate();

            Initially(

                When(CheckoutCreated)
                    .Then(p =>
                    {
                        Console.WriteLine("StateMachine: CheckoutHasCreated");
                        p.Instance.ProductId = p.Data.ProductId;
                        p.Instance.DeliverTo = p.Data.CheckoutBy;
                    })
                .ThenAsync(p =>
                        SendCommand(p,new DoPaymentCommand
                        {
                            CorrelationId = p.Data.CorrelationId,
                            CheckoutId = p.Data.CheckoutId,
                            Amount = p.Data.Amount
                        }))
                .TransitionTo(Processing)
            );


            this.During(Processing, PaymentPerformedHandler());


            When(ProductDelivered)
                .Then(p => { Console.WriteLine($"All process done for coreelation Id {p.Data.CorrelationId}"); })
                .Finalize();

        }

        private EventActivityBinder<CheckoutState, PaymentPerformed> PaymentPerformedHandler() =>
            When(PaymentPerformed)
                .Then(p => { Console.WriteLine("StateMachine: PaymentPerformed"); })
                //.ThenAsync(p => p.Send(new DeliverProductCommand
                //{
                //    CorrelationId = p.Data.CorrelationId,
                //    CheckoutId = p.Data.CheckoutId,
                //    ProductId = p.Instance.ProductId,
                //    DeliveryTo = p.Instance.DeliverTo
                //}))
                .TransitionTo(Paid);

        private async Task SendCommand<TCommand>( BehaviorContext<CheckoutState, CheckoutCreated> context, TCommand message)
            where TCommand : class
        {
            var sendEndpoint = await context.GetSendEndpoint(new Uri("rabbitmq://localhost/saga-101_payment"));
            await sendEndpoint.Send(message);
        }

        private void Correlate()
        {
            Event(() => CheckoutCreated, p => p.CorrelateById(m => m.Message.CorrelationId));
            Event(() => PaymentPerformed, p => p.CorrelateById(m => m.Message.CorrelationId));
            Event(() => ProductDelivered, p => p.CorrelateById(m => m.Message.CorrelationId));
        }

        public Event<CheckoutCreated> CheckoutCreated { get; private set; }
        public Event<PaymentPerformed> PaymentPerformed { get; private set; }
        public Event<ProductDelivered> ProductDelivered { get; private set; }

        public State Processing { get; private set; }
        public State Paid { get; private set; }
    }
}
