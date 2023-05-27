// See https://aka.ms/new-console-template for more information

using Confluent.Kafka;
using System;

class Program
{
    static async Task Main(string[] args)
    {
        // Program execution starts here
        Console.WriteLine("Hello, world!");

        // Rest of your program logic goes here

        // Wait for user input before exiting
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };
        string kafkaTopic = "GameAndPlayerStateBeforeAction";
        string message = $"hw";

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
        }    }
}


