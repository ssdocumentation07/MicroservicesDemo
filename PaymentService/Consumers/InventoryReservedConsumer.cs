using MassTransit;
using Messaging.Events;
using OrderSaga.SagaStates;

namespace PaymentService.Consumers
{
    public class InventoryReservedConsumer : IConsumer<InventoryReserved>
    {

        private readonly IPublishEndpoint _publishEndpoint;

        public InventoryReservedConsumer(
            IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<InventoryReserved> context)
        {
            try
            {
                var message = context.Message;

                Console.WriteLine(
                    $"Inventory Reserved for Order: {message.OrderId}");

                Console.WriteLine(
                    $"Product: {message.ProductId}");

                Console.WriteLine(
                    $"Quantity: {message.Quantity}");

                Console.WriteLine(
                    $"Correlation ID: {message.CorrelationId}");

                // Payment processing simulation
                Console.WriteLine("Processing Payment...");




                #region // Simulate payment failure for saga
                throw new Exception("Payment failed - insufficient balance");
                #endregion

                #region// Ye ab execute nahi hoga agar exception throw ho rha ha

                //await _publishEndpoint.Publish(
                //    new PaymentCompleted
                //    {
                //        CorrelationId = message.CorrelationId,
                //        OrderId = message.OrderId,
                //        Amount = 1500
                //    });


                //Console.WriteLine("Payment Successful");
                //Console.WriteLine(
                //    "PaymentCompleted event published");
                #endregion
            }
            catch (Exception ex)
            {

                Console.WriteLine("Payment failed!");
                Console.WriteLine("Release Inventory");
                Console.WriteLine("Cancel Order");
                await context.Publish(new PaymentFailed
                {
                    CorrelationId = context.Message.CorrelationId,
                    OrderId = context.Message.OrderId,
                    Reason = ex.Message
                });
            }
            
        }
    }
}
