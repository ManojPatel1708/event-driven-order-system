using Confluent.Kafka;
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

Console.WriteLine("💳 Payment Service started...");

while (true)
{
    var consumeResult = consumer.Consume();

    var order = JsonSerializer.Deserialize<dynamic>(consumeResult.Message.Value);

    Console.WriteLine($"Processing payment for Order: {order.OrderId}");

    var paymentEvent = new
    {
        OrderId = order.OrderId,
        Status = "PAYMENT_SUCCESS"
    };

    await producer.ProduceAsync("payment-success", new Message<Null, string>
    {
        Value = JsonSerializer.Serialize(paymentEvent)
    });
}
