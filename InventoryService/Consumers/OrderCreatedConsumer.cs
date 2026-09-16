using MassTransit;
using Messaging.Events;
using OrderSaga.SagaStates;

namespace InventoryService.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreated>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderCreatedConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<OrderCreated> context)
        {
            var order = context.Message;

            Console.WriteLine($"Order Received: {order.OrderId}");
            Console.WriteLine($"Customer: {order.CustomerId}");
            Console.WriteLine($"Amount: {order.Amount}");
            Console.WriteLine($"Correlation Id: {order.CorrelationId}");
            Console.WriteLine("Checking inventory...");

            await _publishEndpoint.Publish(
                                            new InventoryReserved
                                            {
                                                CorrelationId = order.CorrelationId,
                                                OrderId = order.OrderId,
                                                ProductId = 101,
                                                Quantity = 50
                                            });

            Console.WriteLine("InventoryReserved event published");
            await Task.CompletedTask;

        }

        public class ReleaseInventoryConsumer : IConsumer<ReleaseInventory>
        {
            public async Task Consume(ConsumeContext<ReleaseInventory> context)
            {
                var message = context.Message;

                Console.WriteLine("===== RELEASE INVENTORY =====");

                Console.WriteLine(
                    $"OrderId: {message.OrderId}");
                Console.WriteLine(
                    $"OrderId: {message.CorrelationId}");



                await Task.CompletedTask;
            }
        }
    }
}
