using Confluent.Kafka;
using System.Text.Json;

var config = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "notification-group-v3",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

consumer.Subscribe("payment-success");

Console.WriteLine("✅ Notification Service running...");
Console.WriteLine("👂 Waiting for payment events...");

while (true)
{
    var result = consumer.Consume(TimeSpan.FromSeconds(5));

    if (result == null)
    {
        Console.WriteLine("⌛ No messages yet...");
        continue;
    }

    Console.WriteLine($"📩 Received: {result.Message.Value}");

    var payment = JsonSerializer.Deserialize<Payment>(result.Message.Value);

    if (payment != null)
    {
        Console.WriteLine($"📧 Sending notification for Order: {payment.OrderId}");
    }
    else
    {
        Console.WriteLine("❌ Failed to deserialize");
    }
}