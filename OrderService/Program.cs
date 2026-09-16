using MassTransit;
using Messaging.Events;
using OrderSaga.StateMachines;
using OrderSaga.SagaStates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
  // Register Saga
    x.AddSagaStateMachine<OrderStateMachine, OrderState>()
        .InMemoryRepository();

    // regiter RabbitMq
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        // OrderCreated -> order-exchange
        //cfg.Message<OrderCreated>(x =>
        //{
        //    x.SetEntityName("order-exchange");
        //});

        //cfg.Publish<OrderCreated>(x =>
        //{
        //    x.ExchangeType = "direct";
        //    x.Durable = true;
        //});
        // VERY IMPORTANT
        cfg.ConfigureEndpoints(context);
    });
    // regiter RabbitMq
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
