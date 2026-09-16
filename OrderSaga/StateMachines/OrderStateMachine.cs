using MassTransit;
using Messaging.Events;
using OrderSaga.SagaStates;

namespace OrderSaga.StateMachines;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    public State InventoryPending { get; private set; } = null!;

    public State PaymentPending { get; private set; } = null!;

    public Event<OrderCreated> OrderCreated { get; private set; } = null!;

    public Event<InventoryReserved> InventoryReserved { get; private set; } = null!;

    public Event<PaymentCompleted> PaymentCompleted { get; private set; } = null!;
    public Event<PaymentFailed> PaymentFailed { get; private set; } = null!;

    public OrderStateMachine()
    {

        InstanceState(x => x.CurrentState);

        Event(() => OrderCreated,
            e => e.CorrelateById(
                context => context.Message.CorrelationId));

        Event(() => InventoryReserved,
            e => e.CorrelateById(
                context => context.Message.CorrelationId));

        Event(() => PaymentCompleted,
            e => e.CorrelateById(
                context => context.Message.CorrelationId));

        Event(() => PaymentFailed);

        Initially(
                When(OrderCreated)
                    .Then(context =>
                    {
                        context.Saga.OrderId =
                            context.Message.OrderId;

                        context.Saga.Amount =
                            context.Message.Amount;

                        Console.WriteLine(
                            $"Saga Started - Order {context.Message.OrderId}");
                    })
                    .TransitionTo(InventoryPending)
            );

        During(
            InventoryPending,
            When(InventoryReserved)
                .Then(context =>
                {
                    Console.WriteLine(
                        $"Inventory Reserved - Order {context.Saga.OrderId}");
                })
                .TransitionTo(PaymentPending)
        );

        During(
            PaymentPending,
            When(PaymentCompleted)
                .Then(context =>
                {
                    Console.WriteLine(
                        $"Payment Completed - Order {context.Saga.OrderId}");
                })
                .Finalize(),
            When(PaymentFailed)
                    .Then(async context =>
                    {
                        Console.WriteLine(
                            $"Payment Failed - Order {context.Saga.OrderId}");

                        await context.Publish(new ReleaseInventory
                        {
                            CorrelationId = context.Saga.CorrelationId,
                            OrderId = context.Saga.OrderId

                        });
                    })
    .Finalize()
        );

        SetCompletedWhenFinalized();



    }
}