using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using Automatonymous;

namespace CoolBrains.CheckoutHost
{
    public class CheckoutState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public State State { get; set; }
        public Guid CheckoutId { get; set; }
        public Guid ProductId { get; set; }
        public Guid DeliverTo { get; set; }

    }
}
