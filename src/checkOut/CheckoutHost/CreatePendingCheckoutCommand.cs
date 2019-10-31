using System;
using System.Collections.Generic;
using System.Text;

namespace CoolBrains.CheckoutHost
{
    public class CreatePendingCheckoutCommand
    {
        public CreatePendingCheckoutCommand()
        {
            CorrelationId = Guid.NewGuid();
        }
        
        public Guid CheckoutId { get; set; }
        public Guid CorrelationId { get; set; }
        public Guid ProductId { get; set; }
        public decimal Amount { get; set; }
        public Guid CheckoutBy { get; set; }
    }
}
