using Confluent.Kafka;
using System.Text.Json;

var config = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "notification-group",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

consumer.Subscribe("payment-success");

Console.WriteLine("📧 Notification Service running...");

while (true)
{
    var consumeResult = consumer.Consume();

    var payment = JsonSerializer.Deserialize<dynamic>(consumeResult.Message.Value);

    Console.WriteLine($"✅ Sending notification for Order: {payment.OrderId}");
}
