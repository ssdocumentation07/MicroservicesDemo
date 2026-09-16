using MassTransit;

namespace OrderSaga.SagaStates;

public class OrderState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public string CurrentState { get; set; } = string.Empty;

    public int OrderId { get; set; }

    public decimal Amount { get; set; }
}