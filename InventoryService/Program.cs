using InventoryService.Consumers;
using MassTransit;
using Messaging.Events;
using static InventoryService.Consumers.OrderCreatedConsumer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    // Register Consumer
    x.AddConsumer<OrderCreatedConsumer>();
    x.AddConsumer<ReleaseInventoryConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        // RabbitMQ connection
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // Queue
        cfg.ReceiveEndpoint("inventory-order-created", e =>
        {
            //e.ConfigureConsumeTopology = false;
            //e.Bind("order-exchange", x =>
            //{
            //    x.ExchangeType = "direct";
            //    x.RoutingKey = "order.created";
            //});
            e.ConfigureConsumer<OrderCreatedConsumer>(context);
        });
        // inventory reserve -exchange
        //cfg.Message<InventoryReserved>(x =>
        //{
        //    x.SetEntityName("inventory-exchange");
        //});
        //cfg.Publish<InventoryReserved>(x =>
        //{
        //    x.ExchangeType = "direct";
        //    x.Durable = true;
        //});
        cfg.ConfigureEndpoints(context);
    });
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
