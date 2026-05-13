using Confluent.Kafka;
using System.Text.Json;
using Confluent.Kafka.Admin;

var config = new ProducerConfig
{
    BootstrapServers = "localhost:9092",
    Acks = Acks.All,
    MessageTimeoutMs = 5000
};

//use below to create topic Manually
//docker exec -i kafka bash -c "kafka-topics --create --topic order-created --bootstrap-server localhost:9092 --if-not-exists && kafka-topics --create --topic payment-success --bootstrap-server localhost:9092 --if-not-exists && kafka-topics --create --topic payment-retry --bootstrap-server localhost:9092 --if-not-exists && kafka-topics --create --topic payment-dlq --bootstrap-server localhost:9092 --if-not-exists"

// Verify using this id topic created or not 
//docker exec -it kafka kafka-topics --list --bootstrap-server localhost:9092
//docker exec -it kafka kafka-topics --list --bootstrap-server kafka:9092

//Check of order created in Topic r not
//docker exec -it kafka kafka-console-consumer --topic order-created --from-beginning --bootstrap-server kafka:9092

//Run Kafka consumer (FIXED) console to verify messages in topic
//docker exec -it kafka kafka-console-consumer --topic order-created --from-beginning --bootstrap-server kafka:29092

using var adminClient = new AdminClientBuilder(config).Build();

try
{
    await adminClient.CreateTopicsAsync(new[]
    {
        new TopicSpecification
        {
            Name = "order-created",
            NumPartitions = 1,
            ReplicationFactor = 1
        },
        new TopicSpecification
        {
            Name = "payment-success",
            NumPartitions = 1,
            ReplicationFactor = 1
        },
        new TopicSpecification
        {
            Name = "payment-retry",
            NumPartitions = 1,
            ReplicationFactor = 1
        },
        new TopicSpecification
        {
            Name = "payment-dlq",
            NumPartitions = 1,
            ReplicationFactor = 1
        }
    });

    Console.WriteLine("✅ Topics created successfully");
}
catch (CreateTopicsException ex)
{
    Console.WriteLine("⚠ Topics may already exist");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ ERROR creating topics: {ex.Message}");
}


using var producer = new ProducerBuilder<Null, string>(config).Build();

var order = new
{
    OrderId = $"ORD-{DateTime.Now.Ticks}",
    Amount = 1000,
    Status = "CREATED"
};

var json = JsonSerializer.Serialize(order);

Console.WriteLine("➡ Sending message to Kafka...");
Console.WriteLine($"Payload: {json}");

try
{
    var result = await producer.ProduceAsync("order-created", new Message<Null, string>
    {
        Value = json
    });

    Console.WriteLine($"✅ Order Created: {order.OrderId}");
    Console.WriteLine($"📍 Delivered to topic: {result.Topic}, partition: {result.Partition}, offset: {result.Offset}");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ ERROR while sending: {ex.Message}");
}

producer.Flush(TimeSpan.FromSeconds(5));