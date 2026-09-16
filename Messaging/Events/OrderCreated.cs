using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Events
{
    public record OrderCreated
    {
        //Why? Saga ko identify karna hai: Ye event kis Saga instance ka hai? CorrelationId uska unique ID hai.
        public Guid CorrelationId { get; init; } 
        public int OrderId { get; init; }
        public int CustomerId { get; init; }
        public decimal Amount { get; init; }
    }
}
