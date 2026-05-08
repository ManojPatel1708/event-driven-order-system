using Confluent.Kafka;
using System.Text.Json;

var config = new ProducerConfig
{
    BootstrapServers = "localhost:9092"
};

using var producer = new ProducerBuilder<Null, string>(config).Build();

var order = new
{
    OrderId = $"ORD-{DateTime.Now.Ticks}",
    Amount = 1000,
    Status = "CREATED"
};

var message = new Message<Null, string>
{
    Value = JsonSerializer.Serialize(order)
};

await producer.ProduceAsync("order-created", message);

Console.WriteLine($"✅ Order Created: {order.OrderId}");
