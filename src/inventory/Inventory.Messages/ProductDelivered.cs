using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Messages
{
    public class ProductDelivered
    {
        public Guid CheckoutId { get; set; }
        public Guid CorrelationId { get; set; }
        public Guid ProductId { get; set; }
        public Guid DeliveryTo { get; set; }
    }
}
