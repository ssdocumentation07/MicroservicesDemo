using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSaga.SagaStates
{
    public class PaymentFailed
    {
        public Guid CorrelationId { get; set; }
        public int OrderId { get; set; }
        public string Reason { get; set; } = null!;
    }
}
