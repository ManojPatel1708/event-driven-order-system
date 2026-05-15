using Confluent.Kafka;
using PaymentService.Models;
using System.Text.Json;

var consumerConfig = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "payment-group",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

var producerConfig = new ProducerConfig
{
    BootstrapServers = "localhost:9092"
};

using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();

consumer.Subscribe("order-created");

Console.WriteLine("Payment Service started...");
Console.WriteLine("Waiting for messages...");

while (true)
{
    try
    {
        var consumeResult = consumer.Consume(TimeSpan.FromSeconds(5));

        if (consumeResult == null)
        {
            Console.WriteLine("No messages yet...");
            continue;
        }

        Console.WriteLine($"Received: {consumeResult.Message.Value}");

        var order = JsonSerializer.Deserialize<Order>(consumeResult.Message.Value);

        if (order == null)
        {
            Console.WriteLine("Failed to deserialize");
            continue;
        }

        Console.WriteLine($"Processing Order: {order.OrderId}");

        var paymentEvent = new
        {
            OrderId = order.OrderId,
            Status = "PAYMENT_SUCCESS"
        };

        await producer.ProduceAsync("payment-success", new Message<Null, string>
        {
            Value = JsonSerializer.Serialize(paymentEvent)
        });

        Console.WriteLine($"Payment success: {order.OrderId}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR: {ex.Message}");
    }
}
