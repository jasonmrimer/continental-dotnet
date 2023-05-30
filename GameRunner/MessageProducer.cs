using System;
using System.Threading.Tasks;
using Confluent.Kafka;

public class MessageProducer
{
    private string kafkaBootstrapServers;
    private string kafkaTopic;

    public MessageProducer(string bootstrapServers, string topic)
    {
        kafkaBootstrapServers = bootstrapServers;
        kafkaTopic = topic;
    }

    public async Task ProduceMessageAsync(string message)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = kafkaBootstrapServers
        };

        var producer = new ProducerBuilder<Null, string>(config).Build();

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