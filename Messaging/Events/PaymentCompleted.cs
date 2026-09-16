using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Events
{
    public record PaymentCompleted
    {
        public Guid CorrelationId { get; init; }
        public int OrderId { get; init; }
        public decimal Amount { get; init; }
    }
}
