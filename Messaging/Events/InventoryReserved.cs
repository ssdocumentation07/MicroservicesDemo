using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Events
{
    public record InventoryReserved
    {
        public Guid CorrelationId { get; init; }
        public int OrderId { get; init; }

        public int ProductId { get; init; }

        public int Quantity { get; init; }
    }
}
