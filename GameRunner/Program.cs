using Confluent.Kafka;
using Newtonsoft.Json;

class Program
{
    private const string BootstrapServers = "localhost:9092";
    private static string _producerTopic = "LoopCounter";
    private static string _consumerTopic = "LoopCounterResponse";
    private const int MaxCounter = 6;

    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World! from dotnet");
        _producerTopic = args[0];
        _consumerTopic = args[1];
        
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
        consumer.Subscribe(_consumerTopic);

        int counter = 1;
        string programId = Guid.NewGuid().ToString();

        while (counter < MaxCounter)
        {
            // Produce a message with the counter value
            var message = new
            {
                ProgramId = programId,
                LoopCounter = counter
            };

            // Convert the message object to JSON
            var messageJson = JsonConvert.SerializeObject(message);

            var kafkaMessage = new Message<string, string> { Key = null, Value = messageJson };
            producer.Produce(_producerTopic, kafkaMessage);

            // Wait for the response message from the Python app
            var response = consumer.Consume();

            // Deserialize the response JSON
            var responseMessage = JsonConvert.DeserializeObject<ResponseMessage>(response.Message.Value);

            // Check if the program ID matches
            if (responseMessage.ProgramId == programId)
            {
                Console.WriteLine($"Dotnet Received response: {responseMessage.LoopCounter}");

                // Update the counter value based on the response
                counter = responseMessage.LoopCounter + 1;
            }
            else
            {
                Console.WriteLine($"Dotnet Invalid response format: {response.Message.Value}");
            }
        }

        producer.Flush();
        producer.Dispose();
        consumer.Close();

        Console.WriteLine("dotnet Finished");
    }
}

internal class ResponseMessage
{
    public string ProgramId { get; set; }
    public int LoopCounter { get; set; }
}