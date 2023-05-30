﻿// See https://aka.ms/new-console-template for more information

using Confluent.Kafka;
using Newtonsoft.Json;

namespace ConsoleApp1;

class Program
{
    private const string BootstrapServers = "localhost:9092";
    private const string ProducerTopic = "LoopCounter";
    private const string ConsumerTopic = "LoopCounterResponse";
    private const int MaxCounter = 6;

    static void Main(string[] args)
    {
        // Program execution starts here
        Console.WriteLine("Hello, from dotnet!");
        
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = BootstrapServers
        };
        
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = BootstrapServers,
            GroupId = Guid.NewGuid().ToString(),
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        var producer = new ProducerBuilder<string, string>(producerConfig).Build();
        var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        consumer.Subscribe(ConsumerTopic);
        
        int counter = 1;
        string programId = Guid.NewGuid().ToString();
        
        while (counter <= MaxCounter)
        {
            // Produce a message with the counter value
            var message = new
            {
                ProgramId = programId,
                LoopCounter = counter
            };
        
            // Convert the message object to JSON
            var messageJson = JsonConvert.SerializeObject(message);
        
            var kafkaMessage = new Message<string, string> { Key = null!, Value = messageJson };
            producer.Produce(ProducerTopic, kafkaMessage);
        
            // Wait for the response message from the Python app
            var response = consumer.Consume();
        
            // Deserialize the response JSON
            var responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(response.Message.Value);
        
            // Check if the program ID matches
            if (responseMessage != null && responseMessage.ProgramId == programId)
            {
                Console.WriteLine($"Received response: {responseMessage.LoopCounter}");
        
                // Update the counter value based on the response
                counter = responseMessage.LoopCounter + 1;
            }
            else
            {
                Console.WriteLine($"Invalid response format: {response.Message.Value}");
            }
        }
        
        producer.Flush();
        producer.Dispose();
        consumer.Close();

        Console.WriteLine("Finished");
        // return Task.CompletedTask;
    }

    private class ResponseMessage
    {
        public string? ProgramId { get; set; }
        public int LoopCounter { get; set; }
    }
}