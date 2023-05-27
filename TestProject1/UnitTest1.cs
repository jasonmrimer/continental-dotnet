using Confluent.Kafka;

namespace TestProject1;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Test1()
    {
        // See https://aka.ms/new-console-template for more information
        Console.WriteLine("Hello, test!");

        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };

        string kafkaTopic = "GameAndPlayerStateBeforeAction";
        string message = $"testing";

        var producer = new ProducerBuilder<Null, string>(config).Build();
        for (int i = 0; i < 10; i++)
        {
            message += i;

            try
            {
                var deliveryReport =
                    await producer.ProduceAsync(kafkaTopic, new Message<Null, string> { Value = message });
                Console.WriteLine($"Message delivered to '{deliveryReport.TopicPartitionOffset}'");
            }
            catch (ProduceException<Null, string> ex)
            {
                Console.WriteLine($"Delivery failed: {ex.Error.Reason}");
            }
        }
    }
}