using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Messages
{
    public class DoPaymentCommand
    {
        public Guid CheckoutId { get; set; }
        public Guid CorrelationId { get; set; }
        public decimal Amount { get; set; }
        
    }
}
